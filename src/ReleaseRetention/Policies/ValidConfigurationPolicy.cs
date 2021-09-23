using System;
using ReleaseRetention.Abstractions.Model;
using Retention.Abstractions;

namespace ReleaseRetention.Policies
{
    /// <summary>
    ///     Used to check to see if a release is in a valid configuration
    /// </summary>
    [Obsolete("Not implemented")]
    public class ValidConfigurationPolicy : IRetentionPolicy<IRelease>
    {
        /// <summary>
        ///     Invokes the policy against a release.
        /// </summary>
        /// <param name="context">
        ///     The retention context.
        /// </param>
        /// <param name="item">
        ///     The release.
        /// </param>
        /// <returns>
        ///     A <see cref="IRetentionResult{IRelease}" /> with the result of this policy.
        /// </returns>
        public IRetentionResult<IRelease> Invoke(IRetentionContext<IRelease> context, IRelease item)
        {
            throw new NotImplementedException();
        }
    }
}