using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReleaseRetention.Abstractions;
using ReleaseRetention.Abstractions.Model;
using Retention;
using Retention.Abstractions;

namespace ReleaseRetention
{
    public class ReleaseRetention : IReleaseRetention
    {
        /// <summary>
        ///     The source of releases.
        /// </summary>
        private readonly IReleaseSource _releaseSource;
        
        /// <summary>
        ///     Handles the removal of releases after policies are applied.
        /// </summary>
        private readonly IReleaseRemovalHandler _removalHandler;
        
        /// <summary>
        ///     The logger for ReleaseRetention.
        /// </summary>
        private readonly ILogger<ReleaseRetention> _logger;
        
        /// <summary>
        ///     PolicyManager
        /// </summary>
        public IReleaseRetentionPolicyManager PolicyManager { get; }
        
        /// <summary>
        ///     Constructor for Release Retention implementation.
        /// </summary>
        /// <param name="policyManager"></param>
        /// <param name="releaseSource"></param>
        /// <param name="removalHandler"></param>
        /// <param name="logger"></param>
        public ReleaseRetention(
            IReleaseRetentionPolicyManager policyManager,
            IReleaseSource releaseSource,
            IReleaseRemovalHandler removalHandler,
            ILogger<ReleaseRetention> logger)
        {
            PolicyManager = policyManager;
            _releaseSource = releaseSource; // Can also change these to parameters to RunAsync
            _removalHandler = removalHandler; // Can change this to return of RunAsync
            _logger = logger;
        }

        /// <summary>
        ///     Run ReleaseRetention getting all releases and removing what needs to be removed.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            using var logScope = this._logger.BeginScope("ReleaseRetention RunAsync");
            // Get all releases
            
            this._logger.LogTrace("Getting all releases");
            var releases = (await this._releaseSource.GetAsync(cancellationToken)).ToList();
            this._logger.LogInformation("{} Releases found", releases.Count);
            
            // create a retention context
            this._logger.LogTrace("Creating retention context");
            var retentionContext = new RetentionContext<IRelease>(this.PolicyManager, this._logger);
            
            // Apply the policies to all releases
            // (I know this list has to resize, but easier to debug)
            var releaseResults = new List<IRetentionResult<IRelease>>(releases.Count);
            foreach (var release in releases)
            {
                this._logger.LogInformation("Running Release Retention on {}", release);
                var policyForRelease = this.PolicyManager.Get(release);

                // If there is no policy then it does not match any, remove
                if (policyForRelease is null)
                {
                    this._logger.LogInformation("Found no policy for release {}, removing", release);
                    continue;
                }

                // Invoke the found policy against the release
                this._logger.LogTrace("Found policy for {}, {}, invoking", release, policyForRelease);
                var result = policyForRelease.Invoke(retentionContext, release);

                // Add the result back to the list
                // Should use maybe a log formatter here for larger results
                this._logger.LogInformation("Release {} resulted in {}", release, result);
                releaseResults.Add(result);
            }

            // Get releases with unsuccessful policies
            var releasesToRemove = releaseResults
                .Where(r => !r.Success)
                .Select(r => r.Value)
                .ToList(); // Prevent multiple enumeration
            
            this._logger.LogInformation("Removing {} releases", releasesToRemove.Count);
            await this._removalHandler.Remove(releasesToRemove);
            this._logger.LogInformation("Finished removing releases");
        }
    }
}