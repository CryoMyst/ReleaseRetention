using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Retention.Abstractions;
using Retention.Policies;

namespace Retention
{
    /// <summary>
    ///     Used to calculate retentions results for collections of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Retention<T> : IRetention<T>
    {
        private readonly ILogger<Retention<T>> _logger;
        
        /// <summary>
        ///     The retention policy manager for this context
        /// </summary>
        public IRetentionPolicyManager<T> PolicyManager { get; }

        /// <summary>
        ///     The constructor for <see cref="Retention"/>
        /// </summary>
        /// <param name="policyManager">
        ///     The policy manager implementation
        /// </param>
        /// <param name="logger"></param>
        public Retention(
            IRetentionPolicyManager<T> policyManager,
            ILogger<Retention<T>> logger)
        {
            this.PolicyManager = policyManager;
            this._logger = logger;
        }

        /// <summary>
        ///     Get a retention result for a particular item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IRetentionResult<T> Retent(T item)
        {
            // Setup the logging scope
            using var logScope = this._logger.BeginScope(item);
            
            // null items are never retained
            if (item is null)
            {
                return new RetentionResult<T>(item, false, null);
            }

            // Get the retention policy for this item
            var retentionPolicy = this.PolicyManager.Get(item);

            // If the item does not have a retention policy then it should not be retained.
            if (retentionPolicy is null)
            {
                return new RetentionResult<T>(item, false, null);
            }
            
            // Create a retention context allowing for metadata about this invokation to be 
            // passed down the policy chain
            var context = new RetentionContext<T>(this.PolicyManager, this._logger);

            // Return the policy result
            return retentionPolicy.Invoke(context, item);
        }

        public IEnumerable<IRetentionResult<T>> RetentRange(IEnumerable<T> items)
        {
            // Basic implementation for now.
            // Show allowing for a shared context to allow for some caching features
            return items.Select(Retent);
        }
    }
}