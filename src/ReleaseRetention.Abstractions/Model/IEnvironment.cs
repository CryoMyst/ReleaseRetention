using System.Collections.Generic;

namespace ReleaseRetention.Abstractions.Model
{
    /// <summary>
    ///     Shape of an Environment model including navigation properties.
    /// </summary>
    public interface IEnvironment
    {
        string Id { get; }
        string Name { get; }

        ICollection<IDeployment> Deployments { get; }
    }
}