using System.Collections.Generic;

namespace ReleaseRetention.Abstractions.Model
{
    public interface IProject
    {
        string Id { get; }
        string Name { get; }

        ICollection<IRelease> Releases { get; }
    }
}