using System;

namespace ReleaseRetention.Abstractions.Model
{
    /// <summary>
    ///     Shape of a deployment model including navigation properties.
    /// </summary>
    public interface IDeployment
    {
        string Id { get; }
        string ReleaseId { get; }
        string EnvironmentId { get; }
        DateTime DeployedAt { get; }


        IRelease? Release { get; }
        IEnvironment? Environment { get; }
    }
}