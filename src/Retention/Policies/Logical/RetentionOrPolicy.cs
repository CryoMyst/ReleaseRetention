using System;
using System.Linq;
using Retention.Abstractions;

namespace Retention.Policies.Logical
{
    /// <summary>
    ///     Conditional OR based policy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RetentionOrPolicy<T> : IRetentionPolicy<T>
    {
        /// <summary>
        ///     All policies to apply against the item
        /// </summary>
        private readonly IRetentionPolicy<T>[] _policies;
        
        /// <summary>
        ///     Constructor for RetentionOrPolicy.
        /// </summary>
        /// <param name="policies">
        ///     The policies to apply against the item.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     If no policies are passed in.
        /// </exception>
        public RetentionOrPolicy(params IRetentionPolicy<T>[] policies)
        {
            if (!policies.Any()) throw new ArgumentException($"You must have at least one policy.");
            
            this._policies = policies;
        }
        
        /// <summary>
        ///     Invokes the policy against the item, will return a success if ANY of the child policies return success.
        /// </summary>
        /// <param name="context">
        ///     The retention context.
        /// </param>
        /// <param name="item">
        ///     The item to apply the policy to.
        /// </param>
        /// <returns>
        ///     And <see cref="IRetentionResult{T}"/> indicating the success of this policy.
        /// </returns>
        public IRetentionResult<T> Invoke(IRetentionContext<T> context, T item)
        {
            // Gets all the results for all child policies
            // This is not shorted to allow for better logging.
            var policyResults = this._policies.Select(p => p.Invoke(context, item)).ToList();
            
            // Successful if any of the results is is successful
            var isSuccess = policyResults.Any(pr => pr.Success);
            
            // Wrap in a RetentionResult
            return new RetentionResult<T>(item, isSuccess, this, policyResults);
        }
    }
}