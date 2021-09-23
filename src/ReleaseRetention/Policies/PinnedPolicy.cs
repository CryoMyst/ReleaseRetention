using System;
using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention.Policies
{
    /// <summary>
    ///     Used to keep all releases that are pinned
    /// </summary>
    [Obsolete("Not implemented")]
    public class ReleaseRetentionPinnedPolicy : IRetentionPolicy<IRelease>
    {
        /// <summary>
        ///     Invokes the Pinned Policy against a release, NOT IMPLEMENTED
        /// </summary>
        /// <param name="context">
        ///     The retention context.
        /// </param>
        /// <param name="item">
        ///     The release.
        /// </param>
        /// <returns>
        ///     A retention result if the release is pinned.
        /// </returns>
        public IRetentionResult<IRelease> Invoke(IRetentionContext<IRelease> context, IRelease item)
        {
            throw new NotImplementedException();
        }
    }
}