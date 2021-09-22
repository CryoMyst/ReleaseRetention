using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Retention.Abstractions;
using Xunit;

namespace Retention.Tests
{
    public class RetentionUnitTests
    {
        [Fact]
        public void TestReturnNonSuccessOnNullInput()
        {
            // Arrange
            var logger = Mock.Of<ILogger<Retention<object>>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();
            var mockPolicy = new Mock<IRetentionPolicy<object>>();
            
            mockPolicy.Setup(m => m.Invoke(
                    It.IsAny<IRetentionContext<object>>(), 
                    It.IsAny<object>()))
                .Returns(new RetentionResult<object>("NotNullValue", true, mockPolicy.Object));

            mockPolicyManager.Setup(m => m.Get(
                    It.IsAny<object>(),
                    It.IsAny<object?>()))
                .Returns(mockPolicy.Object);

            var retention = new Retention<object>(mockPolicyManager.Object, logger);
            // Act

            var result = retention.Retent(null!);
            // Assert

            result.Policy.Should().BeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeNull();
            result.ChildResults.Should().BeNullOrEmpty();
        }

        [Fact]
        public void TestReturnNonSuccessOnNoPolicy()
        {
            // Arrange
            var input = new object();
            var logger = Mock.Of<ILogger<Retention<object>>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();

            mockPolicyManager.Setup(m => m.Get(
                    It.IsAny<object>(),
                    It.IsAny<object?>()))
                .Returns((IRetentionPolicy<object>?) null);

            var retention = new Retention<object>(mockPolicyManager.Object, logger);
            // Act

            var result = retention.Retent(input);
            // Assert

            result.Policy.Should().BeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().Be(input);
            result.ChildResults.Should().BeNullOrEmpty();
        }

        [Fact]
        public void TestAppliesPolicy()
        {
            // Arrange
            var input = new object();
            var logger = Mock.Of<ILogger<Retention<object>>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();
            var mockPolicy = new Mock<IRetentionPolicy<object>>();
            
            mockPolicy.Setup(m => m.Invoke(
                    It.IsAny<IRetentionContext<object>>(), 
                    It.IsAny<object>()))
                .Returns(new RetentionResult<object>(input, true, mockPolicy.Object));

            mockPolicyManager.Setup(m => m.Get(
                    It.IsAny<object>(),
                    It.IsAny<object?>()))
                .Returns(mockPolicy.Object);

            var retention = new Retention<object>(mockPolicyManager.Object, logger);
            // Act

            var result = retention.Retent(input);
            // Assert

            result.Policy.Should().Be(mockPolicy.Object);
            result.Success.Should().BeTrue();
            result.Value.Should().Be(input);
            result.ChildResults.Should().BeNullOrEmpty();
        }
    }
}