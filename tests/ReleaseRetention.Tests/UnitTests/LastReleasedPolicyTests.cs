using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReleaseRetention.Abstractions.Model;
using ReleaseRetention.Policies;
using ReleaseRetention.Tests.Model;
using Retention;
using Retention.Abstractions;
using Xunit;
using Environment = ReleaseRetention.Tests.Model.Environment;
using Version = ReleaseRetention.Tests.Model.Version;

namespace ReleaseRetention.Tests.UnitTests
{
    /// <summary>
    ///     Tests for the LastRelease Policy
    /// </summary>
    public class LastReleasedPolicyTests
    {
        /// <summary>
        ///     Ensures that if there is no project then the release is non-successful
        /// </summary>
        [Fact]
        public void TestFailAgainstNoProject()
        {
            // Arrange
            var context = new ModelContext();
            
            var release = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
            };
            context.Releases.Add(release);
            
            var deployment = new Deployment()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                DeployedAt = DateTime.Now,
                ReleaseId = release.Id,
            };
            context.Deployments.Add(deployment);

            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(10);
            // Act

            var result = lastReleasedPolicy.Invoke(mockContext.Object, release);
            // Assert

            result.Success.Should().BeFalse();
            result.Policy.Should().Be(lastReleasedPolicy);
            result.Value.Should().Be(release);
            result.ChildResults.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void TestFailAgainstNoDeployments()
        {
            // Arrange
            var context = new ModelContext();
            var project = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestProject",
            };
            context.Projects.Add(project);
            
            var release = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release);
            
            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(10);
            // Act

            var result = lastReleasedPolicy.Invoke(mockContext.Object, release);
            // Assert

            result.Success.Should().BeFalse();
            result.Policy.Should().Be(lastReleasedPolicy);
            result.Value.Should().Be(release);
            result.ChildResults.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void TestFailWhenOutsideKeepNumber()
        {
            // Arrange
            var context = new ModelContext();
            var environment = new Environment()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestEnvironment",
            };
            context.Environments.Add(environment);
            
            var project = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestProject",
            };
            context.Projects.Add(project);
            
            var release1 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release1);
            var release2 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release2);
            var release3 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release3);

            var deployment1 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release1.Id,
                DeployedAt = DateTime.Today,
            };
            context.Deployments.Add(deployment1);
            var deployment2 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release2.Id,
                DeployedAt = DateTime.Today + TimeSpan.FromDays(1),
            };
            context.Deployments.Add(deployment2);
            var deployment3 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release3.Id,
                DeployedAt = DateTime.Today + TimeSpan.FromDays(2),
            };
            context.Deployments.Add(deployment3);


            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(2);
            // Act

            var result = lastReleasedPolicy.Invoke(mockContext.Object, release1);
            // Assert

            result.Success.Should().BeFalse();
            result.Policy.Should().Be(lastReleasedPolicy);
            result.Value.Should().Be(release1);
            result.ChildResults.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void TestSuccessWhenInsideKeepNumber()
        {
            // Arrange
            var context = new ModelContext();
            var environment = new Environment()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestEnvironment",
            };
            context.Environments.Add(environment);
            
            var project = new Project()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                Name = "TestProject",
            };
            context.Projects.Add(project);
            
            var release1 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release1);
            var release2 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release2);
            var release3 = new Release()
            {
                Context = context,
                Id = Guid.NewGuid().ToString(),
                ProjectId = project.Id,
            };
            context.Releases.Add(release3);

            var deployment1 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release1.Id,
                DeployedAt = DateTime.Today,
            };
            context.Deployments.Add(deployment1);
            var deployment2 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release2.Id,
                DeployedAt = DateTime.Today + TimeSpan.FromDays(1),
            };
            context.Deployments.Add(deployment2);
            var deployment3 = new Deployment()
            {
                Context = context,
                EnvironmentId = environment.Id,
                ReleaseId = release3.Id,
                DeployedAt = DateTime.Today + TimeSpan.FromDays(2),
            };
            context.Deployments.Add(deployment3);


            var mockLogger = new Mock<ILogger>();
            var mockContext = new Mock<IRetentionContext<IRelease>>();
            mockContext.Setup(c => c.Logger)
                .Returns(mockLogger.Object);
            var lastReleasedPolicy = new LastReleasedPolicy(3);
            // Act

            var result = lastReleasedPolicy.Invoke(mockContext.Object, release1);
            // Assert

            result.Success.Should().BeTrue();
            result.Policy.Should().Be(lastReleasedPolicy);
            result.Value.Should().Be(release1);
            result.ChildResults.Should().BeNullOrEmpty();
        }
    }
}