using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ReleaseRetention.Abstractions;
using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention
{
    /// <summary>
    ///     The implementation for the policy manager.
    /// </summary>
    public class ReleaseRetentionPolicyManager : IReleaseRetentionPolicyManager
    {
        /// <summary>
        ///     The base policy.
        /// </summary>
        private IRetentionPolicy<IRelease> _basePolicy;
        
        /// <summary>
        ///     The logger for <see cref="ReleaseRetentionPolicyManager"/>
        /// </summary>
        private ILogger<ReleaseRetentionPolicyManager> _logger;
        
        /// <summary>
        ///  Stores all the general state policies
        /// </summary>
        private readonly IDictionary<object, IRetentionPolicy<IRelease>> _baseStatePolicies = new Dictionary<object, IRetentionPolicy<IRelease>>();
        
        /// <summary>
        ///     Stores all the project specific state policies
        /// </summary>
        private readonly IDictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>> _projectStatePolicies = new Dictionary<StatePolicyKey<string>,IRetentionPolicy<IRelease>>();

        /// <summary>
        ///     Stores all the environment specific state policies
        /// </summary>
        private readonly IDictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>> _environmentStatePolicies = new Dictionary<StatePolicyKey<string>,IRetentionPolicy<IRelease>>();
        
        /// <summary>
        ///     Constructor to create a release retention policy manager
        /// </summary>
        /// <param name="basePolicy">
        ///     The required base policy.
        /// </param>
        /// <param name="logger">
        ///     The logger.
        /// </param>
        public ReleaseRetentionPolicyManager(
            IRetentionPolicy<IRelease> basePolicy,
            ILogger<ReleaseRetentionPolicyManager> logger)
        {
            this._basePolicy = basePolicy;
            this._logger = logger;
        }

        /// <summary>
        ///     Sets a base policy for all releases.
        /// </summary>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws if the policy is null as null base policies are not currently supported.
        /// </exception>
        public void SetBasePolicy(IRetentionPolicy<IRelease> policy)
        {
            if (policy is null)
            {
                throw new ArgumentException("Cannot set a null base policy");
            }
            
            this._basePolicy = policy;
        }

        /// <summary>
        ///     Sets a base policy for a state
        /// </summary>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetBaseStatePolicy(object state, IRetentionPolicy<IRelease> policy)
        {
            this._baseStatePolicies[state] = policy;
        }

        /// <summary>
        ///     Sets an overriding policy for a particular project
        /// </summary>
        /// <param name="project">
        ///     The project.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetProjectStatePolicy(IProject project, object state, IRetentionPolicy<IRelease> policy)
        {
            var key = new StatePolicyKey<string>(state, project.Id);
            this._projectStatePolicies[key] = policy;
        }
        
        /// <summary>
        ///     Sets an overriding policy for a particular environment
        /// </summary>
        /// <param name="project">
        ///     The project.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetEnvironmentStatePolicy(IEnvironment environment, object state, IRetentionPolicy<IRelease> policy)
        {
            var key = new StatePolicyKey<string>(state, environment.Id);
            this._environmentStatePolicies[key] = policy;
        }
        
        /// <summary>
        ///     Gets the correct policy for a release
        /// </summary>
        /// <param name="release">
        ///     The release
        /// </param>
        /// <param name="state">
        ///     Additional state
        /// </param>
        /// <returns></returns>
        public IRetentionPolicy<IRelease>? Get(IRelease release, object? state = null)
        {
            // Order of importance, can add more in the future 
            // Project > Base
            // If state is null then looking for base policy

            if (state is null)
            {
                return this._basePolicy;
            }

            IRetentionPolicy<IRelease>? foundPolicy = null;
            // Try and find a start policy for the project
            if (release.ProjectId is not null
                && this._projectStatePolicies.TryGetValue(new StatePolicyKey<string>(state, release.ProjectId),
                    out foundPolicy))
            {
                return foundPolicy;
            }
            
            
            // No overriding found use base
            this._baseStatePolicies.TryGetValue(state, out foundPolicy);
            
            return foundPolicy;
        }

        /// <summary>
        ///     Used as a key to store specific state keys
        /// </summary>
        /// <param name="State">
        ///     The state in the key.
        /// </param>
        /// <param name="Id">
        ///     The id in the key.
        /// </param>
        /// <typeparam name="T">
        ///     The id type.
        /// </typeparam>
        private record StatePolicyKey<T>(object State, T Id);
    }
}