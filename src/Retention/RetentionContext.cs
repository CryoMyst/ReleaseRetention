using Microsoft.Extensions.Logging;
using Retention.Abstractions;

namespace Retention
{
    /// <summary>
    ///     The current invokation context of the retention,
    ///     Used to store metadata through the invokation.
    /// </summary>
    /// <param name="PolicyManager"></param>
    /// <typeparam name="T"></typeparam>
    public record RetentionContext<T>(
        IRetentionPolicyManager<T> PolicyManager,
        ILogger Logger) : IRetentionContext<T>;
}