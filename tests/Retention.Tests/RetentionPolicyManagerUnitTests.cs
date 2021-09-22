using FluentAssertions;
using Moq;
using Retention.Abstractions;
using Xunit;

namespace Retention.Tests
{
    public class RetentionPolicyManagerUnitTests
    {
        [Fact]
        public void TestManagerReturnsBasePolicyOnNullState()
        {
            // Arrange
            var input = new object();
            
            var policyManager = new RetentionPolicyManager<object>();
            var mockBasePolicy = new Mock<IRetentionPolicy<object>>();

            // Act
            policyManager.Set(mockBasePolicy.Object, null);
            var policy = policyManager.Get(input);

            // Assert
            policy.Should().Be(mockBasePolicy.Object);
        }

        [Fact]
        public void TestManagerReturnCorrectStatePolicy()
        {
            // Arrange
            var input = new object();
            var state = "SomeState";
            
            var policyManager = new RetentionPolicyManager<object>();
            var mockBasePolicy = new Mock<IRetentionPolicy<object>>();
            var mockStatePolicy = new Mock<IRetentionPolicy<object>>();

            // Act
            policyManager.Set(mockBasePolicy.Object, null);
            policyManager.Set(mockStatePolicy.Object, state);
            var policy = policyManager.Get(input, state);

            // Assert
            policy.Should().Be(mockStatePolicy.Object);
        }

        [Fact]
        public void TestManagerOverwritesPolicyOnSet()
        {
            // Arrange
            var input = new object();
            
            var policyManager = new RetentionPolicyManager<object>();
            var mockBasePolicy = new Mock<IRetentionPolicy<object>>();
            var mockBasePolicy2 = new Mock<IRetentionPolicy<object>>();

            // Act
            policyManager.Set(mockBasePolicy.Object, null);
            policyManager.Set(mockBasePolicy2.Object, null);
            var policy = policyManager.Get(input);

            // Assert
            policy.Should().Be(mockBasePolicy2.Object);
        }

        [Fact]
        public void TestManagerRemovePolicy()
        {
            // Arrange
            var input = new object();
            
            var policyManager = new RetentionPolicyManager<object>();
            var mockBasePolicy = new Mock<IRetentionPolicy<object>>();

            // Act
            policyManager.Set(mockBasePolicy.Object, null);
            policyManager.Remove(null);
            var policy = policyManager.Get(input);
            
            // Assert
            policy.Should().BeNull();
        }
    }
}