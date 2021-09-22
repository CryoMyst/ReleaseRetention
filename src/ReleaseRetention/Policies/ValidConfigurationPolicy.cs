using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention.Policies
{
    public class ValidConfigurationPolicy : IRetentionPolicy<IRelease>
    {
        public IRetentionResult<IRelease> Invoke(IRetentionContext<IRelease> context, IRelease item)
        {
            throw new System.NotImplementedException();
        }
    }
}