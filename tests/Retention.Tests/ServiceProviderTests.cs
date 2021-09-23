using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Retention.Abstractions;
using Retention.Extensions;
using Xunit;

namespace Retention.Tests
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
                .AddRetention()
                .BuildServiceProvider();
            
            // Act
            var retention = services.GetService<IRetention<object>>();
            var retentionPolicyManager = services.GetService<IRetentionPolicyManager<object>>();

            // Assert
            retention.Should().NotBeNull();
            retentionPolicyManager.Should().NotBeNull();
        }
    }
}