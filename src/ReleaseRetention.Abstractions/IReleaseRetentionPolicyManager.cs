using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     Abstraction for the retention manager for releases.
    /// </summary>
    public interface IReleaseRetentionPolicyManager : IRetentionPolicyManager<IRelease>
    {
        /// <summary>
        ///     Sets a default policy to be used against all releases initially.
        /// </summary>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetBasePolicy(IRetentionPolicy<IRelease> policy);

        /// <summary>
        ///     Sets a policy used when a policy requests a state policy reference.
        /// </summary>
        /// <param name="state">
        ///     The state used to store this policy.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetBaseStatePolicy(object state, IRetentionPolicy<IRelease> policy);

        /// <summary>
        ///     Sets a policy used when a policy requests a state policy reference against a project
        ///     which will overwrite base state policy.
        /// </summary>
        /// <param name="project">
        ///     The project to set this state policy against.
        /// </param>
        /// <param name="state">
        ///     The state used to store this policy.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        public void SetProjectStatePolicy(IProject project, object state, IRetentionPolicy<IRelease> policy);
    }
}