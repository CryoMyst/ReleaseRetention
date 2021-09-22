using System.Collections.Generic;

namespace Retention.Abstractions
{
    /// <summary>
    ///     Abstraction for a retention policy that is applied against an item.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the item being held.
    /// </typeparam>
    public interface IRetentionPolicy<T>
    {
        /// <summary>
        ///     Invokes the policy against a particular item.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        IRetentionResult<T> Invoke(IRetentionContext<T> context, T item);
    }
}
