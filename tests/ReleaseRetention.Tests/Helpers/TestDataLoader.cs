using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using ReleaseRetention.Abstractions.Model;
using ReleaseRetention.Tests.Extensions;
using ReleaseRetention.Tests.Model;

namespace ReleaseRetention.Tests.Helpers
{
    public static class TestDataLoader
    {
        public static TestData Load()
        {
            var deployments = JsonSerializer.Deserialize<List<Deployment>>(File.ReadAllText(@"Data/Deployments.json"))!;
            var environments = JsonSerializer.Deserialize<List<Environment>>(File.ReadAllText(@"Data/Environments.json"))!;
            var releases = JsonSerializer.Deserialize<List<Release>>(File.ReadAllText(@"Data/Releases.json"))!;
            var projects = JsonSerializer.Deserialize<List<Project>>(File.ReadAllText(@"Data/Projects.json"))!;

            var context = new ModelContext()
            {
                Deployments = deployments.OfType<IDeployment>().ToList(),
                Environments = environments.OfType<IEnvironment>().ToList(),
                Releases = releases.OfType<IRelease>().ToList(),
                Projects = projects.OfType<IProject>().ToList()
            };
            
            // Stitch in deployment navigation data
            foreach (var deployment in deployments)
            {
                deployment.Context = context;
            }

            // Stitch in Environment navigation data
            foreach (var environment in environments)
            {
                environment.Context = context;
            }

            // Stitch in release navigation data
            foreach (var release in releases)
            {
                release.Context = context;
            }

            // Stitch in project navigation data
            foreach (var project in projects)
            {
                project.Context = context;
            }

            return new TestData(
                deployments.OfType<IDeployment>().ToList(),
                environments.OfType<IEnvironment>().ToList(),
                releases.OfType<IRelease>().ToList(),
                projects.OfType<IProject>().ToList());
        }
    }
}