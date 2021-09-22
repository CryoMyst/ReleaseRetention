using Microsoft.Extensions.Logging;

namespace Retention.Abstractions
{
    /// <summary>
    ///     The retention context of an invokation, keeps track of data to pass to child policies.
    /// </summary>
    /// <typeparam name="T">
    ///     The item type.
    /// </typeparam>
    public interface IRetentionContext<T>
    {
        /// <summary>
        ///     The policy manager of the invokation.
        /// </summary>
        public IRetentionPolicyManager<T> PolicyManager { get; }
        
        /// <summary>
        ///     The logger for the context.
        /// </summary>
        public ILogger Logger { get; }
    }
}