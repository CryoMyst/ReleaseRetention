using System;
using ReleaseRetention.Abstractions.Model;
using Retention;
using Retention.Abstractions;

namespace ReleaseRetention.Policies
{
    /// <summary>
    ///     Used to keep all releases that are pinned
    /// </summary>
    public class ReleaseRetentionPinnedPolicy : IRetentionPolicy<IRelease>
    {
        public IRetentionResult<IRelease> Invoke(IRetentionContext<IRelease> context, IRelease item)
        {
            throw new NotImplementedException();
        }
    }
}