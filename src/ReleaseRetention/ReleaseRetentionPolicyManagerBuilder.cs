using System;
using System.Collections.Generic;
using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention
{
    /// <summary>
    ///     Used to build the policy manager outside of the DI context.
    /// </summary>
    public class ReleaseRetentionPolicyManagerBuilder
    {
        /// <summary>
        ///     The base policy.
        /// </summary>
        internal IRetentionPolicy<IRelease>? BasePolicy { get; private set; }

        /// <summary>
        ///  Stores all the general state policies
        /// </summary>
        internal IDictionary<object, IRetentionPolicy<IRelease>> BaseStatePolicies { get; }
            = new Dictionary<object, IRetentionPolicy<IRelease>>();

        /// <summary>
        ///     Stores all the project specific state policies
        /// </summary>
        internal IDictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>> ProjectStatePolicies { get; }
            = new Dictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>>();

        /// <summary>
        ///     Stores all the environment specific state policies
        /// </summary>
        internal IDictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>> EnvironmentStatePolicies { get; }
            = new Dictionary<StatePolicyKey<string>, IRetentionPolicy<IRelease>>();

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

            this.BasePolicy = policy;
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
            this.BaseStatePolicies[state] = policy;
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
            this.ProjectStatePolicies[key] = policy;
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
            this.EnvironmentStatePolicies[key] = policy;
        }
    }
}