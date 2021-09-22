namespace Retention.Abstractions
{
    /// <summary>
    ///     Used to maintain all retention policies for the <see cref="IRetention"/> container.
    /// </summary>
    public interface IRetentionPolicyManager<T>
    {
        /// <summary>
        ///     Gets the correct retention policy for a particular item.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="state">
        ///     The optional state.
        /// </param>
        /// <returns></returns>
        public IRetentionPolicy<T>? Get(T item, object? state = null);
    }
}