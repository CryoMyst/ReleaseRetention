using System.Threading;
using System.Threading.Tasks;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     Interface defining the public behavior of the release retention.
    /// </summary>
    public interface IReleaseRetention
    {
        /// <summary>
        ///     The policy manager.
        /// </summary>
        public IReleaseRetentionPolicyManager PolicyManager { get; }

        /// <summary>
        ///     Run the ReleaseRetention process.
        /// </summary>
        /// <param name="cancellationToken">
        ///     The cancellation token.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> to run.
        /// </returns>
        public Task RunAsync(CancellationToken cancellationToken = default);
    }
}