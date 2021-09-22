using System.Collections.Generic;
using Retention.Abstractions;

namespace Retention
{
    /// <summary>
    ///     Record of the retention result from a Policy.
    /// </summary>
    /// <param name="Value">
    ///     The value being held.
    /// </param>
    /// <param name="Success">
    ///     Indicates the success of the item against the policy.
    /// </param>
    /// <typeparam name="T">
    ///     Type of item being held.
    /// </typeparam>
    public record RetentionResult<T>(
        T Value,
        bool Success,
        IRetentionPolicy<T>? Policy,
        IEnumerable<IRetentionResult<T>>? ChildResults = null) : IRetentionResult<T>;
}