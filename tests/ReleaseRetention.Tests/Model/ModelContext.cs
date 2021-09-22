using System.Collections.Generic;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Model
{
    public class ModelContext
    {
        public ICollection<IDeployment> Deployments = new List<IDeployment>();
        public ICollection<IEnvironment> Environments = new List<IEnvironment>();
        public ICollection<IRelease> Releases = new List<IRelease>();
        public ICollection<IProject> Projects= new List<IProject>();
    };
}