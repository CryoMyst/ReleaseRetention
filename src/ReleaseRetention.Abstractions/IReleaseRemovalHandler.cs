using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Abstractions
{
    /// <summary>
    ///     A handler that will be notified when a release needs to be removed.
    /// </summary>
    public interface IReleaseRemovalHandler
    {
        /// <summary>
        ///     Method should handle the removal of releases from the system
        /// </summary>
        /// <param name="releasesToRemove"></param>
        /// <returns></returns>
        public Task Remove(IEnumerable<IRelease> releasesToRemove);
    }
}