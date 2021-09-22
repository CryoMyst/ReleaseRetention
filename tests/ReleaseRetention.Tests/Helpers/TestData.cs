using System.Collections.Generic;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Helpers
{
    public record TestData(
        ICollection<IDeployment> Deployments,
        ICollection<IEnvironment> Environments,
        ICollection<IRelease> Releases,
        ICollection<IProject> Projects);
}