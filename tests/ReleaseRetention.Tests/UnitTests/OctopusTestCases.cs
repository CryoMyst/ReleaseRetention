using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReleaseRetention.Abstractions.Model;
using ReleaseRetention.Policies;
using ReleaseRetention.Tests.Model;
using Retention.Abstractions;
using Xunit;
using Environment = ReleaseRetention.Tests.Model.Environment;
using Version = ReleaseRetention.Tests.Model.Version;

namespace ReleaseRetention.Tests.UnitTests
{
    /// <summary>
    ///     Test cases given by octopus deploy.
    /// </summary>
    public class OctopusTestCases
    {
        [Fact]
        public void TestCase1()
        {
            // Arrange
            var context = new ModelContext();
            // Environment 1
            var environment1 = new Environment()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestEnvironment",
            };
            context.Environments.Add(environment1);
            
            // Project 1
            var project1 = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestProject",
            };
            context.Projects.Add(project1);
            
            var release1 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Version = new Version()
                {
                    Number = new System.Version(1, 0, 0),
                },
                Created = new DateTime(2000, 1, 1),
                ProjectId = project1.Id,
            };
            context.Releases.Add(release1);

            var deployment1 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment1.Id,
                ReleaseId = release1.Id,
                DeployedAt = new DateTime(2000, 1, 1),
            };
            context.Deployments.Add(deployment1);


            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(1);
            // Act

            var result = lastReleasedPolicy.Invoke(mockContext.Object, release1);
            // Assert

            result.Success.Should().BeTrue();
            result.Policy.Should().Be(lastReleasedPolicy);
            result.Value.Should().Be(release1);
            result.ChildResults.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void TestCase2()
        {
            // Arrange
            var context = new ModelContext();
            // Environment 1
            var environment1 = new Environment()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "Environment 1",
            };
            context.Environments.Add(environment1);
            
            // Project 1
            var project1 = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "Project 1",
            };
            context.Projects.Add(project1);
            
            var release1 = new Release()
            {
                Context = context,
                Id = "Release 1",
                Version = new Version()
                {
                    Number = new System.Version(1, 0, 0),
                },
                Created = new DateTime(2000, 1, 1),
                ProjectId = project1.Id,
            };
            context.Releases.Add(release1);
            
            var release2 = new Release()
            {
                Context = context,
                Id = "Release 2",
                Version = new Version()
                {
                    Number = new System.Version(1, 0, 0),
                },
                Created = new DateTime(2000, 1, 1),
                ProjectId = project1.Id,
            };
            context.Releases.Add(release2);

            var deployment1 = new Deployment()
            {
                Context = context,
                Id = "Deployment 1",
                EnvironmentId = environment1.Id,
                ReleaseId = release2.Id,
                DeployedAt = new DateTime(2000, 1, 1),
            };
            context.Deployments.Add(deployment1);
            
            var deployment2 = new Deployment()
            {
                Context = context,
                Id = "Deployment 2",
                EnvironmentId = environment1.Id,
                ReleaseId = release1.Id,
                DeployedAt = new DateTime(2000, 1, 1) + TimeSpan.FromHours(1),
            };
            context.Deployments.Add(deployment2);

            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(1);
            // Act

            var result1 = lastReleasedPolicy.Invoke(mockContext.Object, release1);
            var result2 = lastReleasedPolicy.Invoke(mockContext.Object, release2);
            // Assert

            result1.Success.Should().BeTrue();
            result2.Success.Should().BeFalse();
        }
        
        [Fact]
        public void TestCase3()
        {
            // Arrange
            var context = new ModelContext();
            // Environment 1
            var environment1 = new Environment()
            {
                Context = context,
                Id = "Environment 1",
                Name = "TestEnvironment",
            };
            context.Environments.Add(environment1);
            
            var environment2 = new Environment()
            {
                Context = context,
                Id = "Environment 2",
                Name = "TestEnvironment",
            };
            context.Environments.Add(environment2);
            
            // Project 1
            var project1 = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestProject",
            };
            context.Projects.Add(project1);
            
            var release1 = new Release()
            {
                Context = context,
                Id = "Release1",
                Version = new Version()
                {
                    Number = new System.Version(1, 0, 0),
                },
                Created = new DateTime(2000, 1, 1),
                ProjectId = project1.Id,
            };
            context.Releases.Add(release1);
            
            var release2 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Version = new Version()
                {
                    Number = new System.Version(1, 0, 0),
                },
                Created = new DateTime(2000, 1, 1),
                ProjectId = project1.Id,
            };
            context.Releases.Add(release2);

            var deployment1 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment1.Id,
                ReleaseId = release1.Id,
                DeployedAt = new DateTime(2000, 1, 1),
            };
            context.Deployments.Add(deployment1);
            
            var deployment2 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment2.Id,
                ReleaseId = release2.Id,
                DeployedAt = new DateTime(2000, 1, 1) + TimeSpan.FromHours(1),
            };
            context.Deployments.Add(deployment2);

            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(1);
            // Act

            var result1 = lastReleasedPolicy.Invoke(mockContext.Object, release1);
            var result2 = lastReleasedPolicy.Invoke(mockContext.Object, release2);
            // Assert

            result1.Success.Should().BeTrue();
            result2.Success.Should().BeTrue();
        }
    }
}