using System.Linq;
using Microsoft.Extensions.Logging;
using ReleaseRetention.Abstractions.Model;
using Retention;
using Retention.Abstractions;

namespace ReleaseRetention.Policies
{
    /// <summary>
    ///     Last Released Policy implementation, used to keep the last N releases in an Environment/Project grouping.
    /// </summary>
    public class LastReleasedPolicy : IRetentionPolicy<IRelease>
    {
        /// <summary>
        ///     The number of recent environment/project releases to keep.
        /// </summary>
        private readonly int _numberOfReleasesToKeep;

        /// <summary>
        ///     Constructor for LastReleasedPolicy.
        /// </summary>
        /// <param name="numberOfReleasesToKeep">
        ///     The number of recent environment/project releases to keep.
        /// </param>
        public LastReleasedPolicy(int numberOfReleasesToKeep)
        {
            this._numberOfReleasesToKeep = numberOfReleasesToKeep;
        }

        /// <summary>
        ///     Finds the most recent releases for the project/environment and determines if the current release is in them.
        /// </summary>
        /// <param name="context">
        ///     The retention context
        /// </param>
        /// <param name="release">
        ///     The release.
        /// </param>
        /// <returns></returns>
        public IRetentionResult<IRelease> Invoke(IRetentionContext<IRelease> context, IRelease release)
        {
            var logger = context.Logger;
            using var logScope = logger.BeginScope(nameof(LastReleasedPolicy));

            logger.LogTrace("Checking {} is in last {} deployed", release.Id, this._numberOfReleasesToKeep);
            // Grab the project the release is in
            var project = release.Project;

            // Grab the environments this release is in
            var environments = release.Deployments.Select(d => d.Environment)
                .Where(e => e is not null);

            // Get all releases in the same project/environment
            // Check to see if current release is in top n of any

            foreach (var environment in environments)
            {
                logger.LogTrace("Checking against environment: {}", environment.Id);

                // All releases in same project
                var projectReleases = project?.Releases ?? Enumerable.Empty<IRelease>();

                // All releases in same environment
                var environmentReleases = environment?.Deployments.Select(d => d.Release)
                                          ?? Enumerable.Empty<IRelease>();

                // intersect the results
                var releasesInBothProjectAndEnviroment = projectReleases.Intersect(environmentReleases);

                // Grab the latest N releases from this group
                var latestReleases = releasesInBothProjectAndEnviroment
                    .OrderByDescending(r => r.Deployments.OrderByDescending(d => d.DeployedAt).First().DeployedAt)
                    .Take(this._numberOfReleasesToKeep);

                // Check to see if any match this release id
                var inLatestReleases = latestReleases.Any(r => r.Id == release.Id);

                if (inLatestReleases)
                {
                    logger.LogInformation("{} passed policy because it was in the most recently {} deployed in {}",
                        release.Id,
                        this._numberOfReleasesToKeep,
                        environment.Name);
                    return new RetentionResult<IRelease>(release, true, this);
                }
            }

            logger.LogInformation("{} failed policy because it was in the most recently deployed in any environment",
                release.Id);
            // If none are found return a non-success
            return new RetentionResult<IRelease>(release, false, this);
        }
    }
}