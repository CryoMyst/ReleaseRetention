using System.Threading;
using System.Threading.Tasks;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     Interface defining the public behavior of the release retention.
    /// </summary>
    public interface IReleaseRetention
    {
        public IReleaseRetentionPolicyManager PolicyManager { get; }
        public Task RunAsync(CancellationToken cancellationToken = default);
    }
}