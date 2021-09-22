using System.Collections.Generic;

namespace ReleaseRetention.Abstractions.Model
{
    public interface IEnvironment
    {
        string Id { get; }
        string Name { get; }

        ICollection<IDeployment> Deployments { get; }
    }
}