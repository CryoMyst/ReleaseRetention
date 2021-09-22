using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     Abstraction for the retention manager for releases.
    /// </summary>
    public interface IReleaseRetentionPolicyManager : IRetentionPolicyManager<IRelease>
    {
        public void SetBasePolicy(IRetentionPolicy<IRelease> policy);
        public void SetBaseStatePolicy(object state, IRetentionPolicy<IRelease> policy);
        public void SetProjectStatePolicy(IProject project, object state, IRetentionPolicy<IRelease> policy);
    }
}