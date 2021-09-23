using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ReleaseRetention.Abstractions;
using ReleaseRetention.Abstractions.Model;
using ReleaseRetention.Extensions;
using Retention.Abstractions;
using Xunit;

namespace ReleaseRetention.Tests.IntegrationTests
{
    /// <summary>
    ///     Tests to ensure all required services are registered
    /// </summary>
    public class ServiceProviderTests
    {
        [Fact]
        public void TestAllServicesRegister()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddReleaseRetention<TestReleaseSource, TestReleaseRemovalHandler>()
                .BuildServiceProvider();
            
            // Act
            var retention = services.GetService<IReleaseRetention>();
            var retentionPolicyManager = services.GetService<IReleaseRetentionPolicyManager>();

            // Assert
            retention.Should().NotBeNull();
            retentionPolicyManager.Should().NotBeNull();
        }

        class TestReleaseSource : IReleaseSource
        {
            public Task<IEnumerable<IRelease>> GetAsync(CancellationToken cancellationToken = default)
            {
                throw new System.NotImplementedException();
            }
        }

        class TestReleaseRemovalHandler : IReleaseRemovalHandler
        {
            public Task Remove(IEnumerable<IRelease> releasesToRemove)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}