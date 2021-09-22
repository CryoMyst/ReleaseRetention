using System.Collections.Generic;
using Retention.Abstractions;

namespace Retention
{
    /// <summary>
    ///     A basic policy manager that chooses the correct policy for an item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RetentionPolicyManager<T> : IRetentionPolicyManager<T>
    {
        /// <summary>
        ///     The base policy to apply if none is found 
        /// </summary>
        private IRetentionPolicy<T>? _basePolicy = null;
        
        /// <summary>
        ///     The lookup for state policies
        /// </summary>
        private readonly IDictionary<object, IRetentionPolicy<T>> _statePolicies = new Dictionary<object, IRetentionPolicy<T>>();

        /// <summary>
        ///     Gets the correct policy
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="state">
        ///     An optional current state for this policy
        /// </param>
        /// <returns>
        ///     The correct <see cref="IRetentionPolicy{T}"/> to apply to this item
        /// </returns>
        public IRetentionPolicy<T>? Get(T item, object? state = null)
        {
            // Null states will just return the base policy for this item
            if (state is null)
            {
                return this._basePolicy;
            }
            
            // Otherwise try and find a state retention policy 
            var found = this._statePolicies.TryGetValue(state, out var foundPolicy);
            return foundPolicy;
        }

        /// <summary>
        ///     Used to set a policy for a state
        /// </summary>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        public void Set(IRetentionPolicy<T> policy, object? state = null)
        {
            if (state is null)
            {
                this._basePolicy = policy;
            }
            else
            {
                this._statePolicies[state] = policy;
            }
        }

        /// <summary>
        ///     Removes the policy applied to a particular state
        /// </summary>
        /// <param name="state">
        ///     The state.
        /// </param>
        public void Remove(object? state)
        {
            if (state is null)
            {
                this._basePolicy = null;
            }
            else
            {
                this._statePolicies.Remove(state);
            }
        }
    }
}