using Retention.Abstractions;

namespace Retention.Policies
{
    /// <summary>
    ///     A state retention policy used to grabbing reusable
    ///     policies from the manager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RetentionStatePolicy<T> : IRetentionPolicy<T>
    {
        /// <summary>
        ///     The state of the policy to grab
        /// </summary>
        private readonly object _state;

        /// <summary>
        ///     If failing to find the policy should be a success.
        /// </summary>
        private readonly bool _defaultSuccess;
        
        
        /// <summary>
        ///     The constructor for <see cref="RetentionStatePolicy{T}"/>
        /// </summary>
        /// <param name="state">
        ///     The state of the policy to use.
        /// </param>
        public RetentionStatePolicy(object state, bool defaultSuccess = true)
        {
            this._state = state;
            this._defaultSuccess = defaultSuccess;
        }
        
        /// <summary>
        ///     Invokes the retention policy against the item with the given state.
        ///     This will return a successful result if the a policy is not found.
        /// </summary>
        /// <param name="context">
        ///     The retention context.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <returns>
        ///     A retention result for the item.
        /// </returns>
        public IRetentionResult<T> Invoke(IRetentionContext<T> context, T item)
        {
            // Grab the policy with the _name
            var policy = context.PolicyManager.Get(item, this._state);
            
            // null policy would result in a success
            if (policy is null)
            {
                return new RetentionResult<T>(item, this._defaultSuccess, this);
            }
            
            return policy.Invoke(context, item);
        }
    }
}