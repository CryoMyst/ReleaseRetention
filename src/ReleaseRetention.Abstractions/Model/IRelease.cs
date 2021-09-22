using System;
using System.Collections.Generic;

namespace ReleaseRetention.Abstractions.Model
{
    /// <summary>
    ///     Interface defining the shape of a release model.
    /// </summary>
    public interface IRelease
    {
        string Id { get; }
        string ProjectId { get; }
        IVersion? Version { get; }
        DateTime Created { get; }

        IProject? Project { get; }
        ICollection<IDeployment> Deployments { get; }
    }
}