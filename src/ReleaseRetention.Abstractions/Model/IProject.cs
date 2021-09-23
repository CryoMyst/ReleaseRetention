using System.Collections.Generic;

namespace ReleaseRetention.Abstractions.Model
{
    /// <summary>
    ///     Shape of a project model including navigation properties.
    /// </summary>
    public interface IProject
    {
        string Id { get; }
        string Name { get; }

        ICollection<IRelease> Releases { get; }
    }
}