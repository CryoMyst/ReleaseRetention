using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     Defines an implementation for grabbing releases
    /// </summary>
    public interface IReleaseSource
    {
        /// <summary>
        ///     Gets all releases
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<IRelease>> GetAsync(CancellationToken cancellationToken = default);
    }
}