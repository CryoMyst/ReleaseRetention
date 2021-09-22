using System.Collections.Generic;

namespace Retention.Abstractions
{
    /// <summary>
    ///     Abstraction for the main Retention class
    /// </summary>
    public interface IRetention<T>
    {
        /// <summary>
        ///     The policy manager for this retention context
        /// </summary>
        public IRetentionPolicyManager<T> PolicyManager { get; }

        /// <summary>
        ///     Used to see if the item follows the retention policy
        /// </summary>
        /// <param name="item">
        ///     The item being checked.
        /// </param>
        /// <returns>
        ///     An <see cref="IRetentionResult{T}"/> determining if the value should be retained.
        /// </returns>
        public IRetentionResult<T> Retent(T item);

        /// <summary>
        ///     Used to see if a range of items follows the retention policy.
        ///     Same as calling Retent on each item.
        /// </summary>
        /// <param name="items">
        ///     The items being checked.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{IRetentionResult}"/> determining if the value should be retained.
        /// </returns>
        public IEnumerable<IRetentionResult<T>> RetentRange(IEnumerable<T> items);
    }
}