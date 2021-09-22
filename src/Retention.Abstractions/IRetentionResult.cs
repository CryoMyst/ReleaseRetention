using System.Collections.Generic;

namespace Retention.Abstractions
{
    /// <summary>
    ///     Abstraction for a retention result.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the item being held.
    /// </typeparam>
    public interface IRetentionResult<T>
    {
        /// <summary>
        ///     The value that the policies were run against.
        /// </summary>
        public T Value { get; }
        
        /// <summary>
        ///     Bool indicating if the value passed the retention. 
        /// </summary>
        public bool Success { get; }
        
        /// <summary>
        ///     The policy this result is from
        /// </summary>
        public IRetentionPolicy<T>? Policy { get; }
        
        /// <summary>
        ///     Used to keep track of the success of any children policy results.
        ///     This is useful if the caller wants to analyze the decision tree.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{IRetentionResult}"/> of all child results
        /// </returns>
        public IEnumerable<IRetentionResult<T>>? ChildResults { get; }
    }
}