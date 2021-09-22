using System;

namespace ReleaseRetention.Abstractions.Model
{
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