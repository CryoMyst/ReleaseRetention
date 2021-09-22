using FluentAssertions;
using Moq;
using Retention.Abstractions;
using Retention.Policies;
using Xunit;

namespace Retention.Tests.Policies
{
    /// <summary>
    ///     Used to test the <see cref="RetentionStatePolicy{T}"/>
    /// </summary>
    public class RetentionStatePolicyUnitTests
    {
        [Fact]
        public void TestRetentionStateReturnsFoundPolicy()
        {
            // Arrange
            var input = new object();
            var state = "SomeState";
            
            var mockContext = new Mock<IRetentionContext<object>>();
            var mockPolicy = new Mock<IRetentionPolicy<object>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();
            
            mockPolicy.Setup(m => m.Invoke(
                    It.IsAny<IRetentionContext<object>>(), 
                    It.IsAny<object>()))
                .Returns(new RetentionResult<object>(input, true, mockPolicy.Object));

            mockPolicyManager.Setup(m => m.Get(
                It.IsAny<object>(),
                It.IsAny<object?>()))
                .Returns(mockPolicy.Object);
            
            mockContext.Setup(c => c.PolicyManager)
                .Returns(mockPolicyManager.Object);

            var statePolicy = new RetentionStatePolicy<object>(state);
            // Act

            var result = statePolicy.Invoke(mockContext.Object, input);
            // Assert

            result.Success.Should().Be(true);
            result.Policy.Should().BeEquivalentTo(mockPolicy.Object);
            result.Value.Should().Be(input);
            result.ChildResults.Should().BeNullOrEmpty();
        }

        [Fact]
        public void TestRetentionStateReturnDefaultSuccessOnNotFound()
        {
            // Arrange
            var input = new object();
            var state = "SomeState";
            
            var mockContext = new Mock<IRetentionContext<object>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();

            mockPolicyManager.Setup(m => m.Get(
                    It.IsAny<object>(),
                    It.IsAny<object?>()))
                .Returns((IRetentionPolicy<object>?) null);
            
            mockContext.Setup(c => c.PolicyManager)
                .Returns(mockPolicyManager.Object);
            
            var statePolicy = new RetentionStatePolicy<object>(state, true);
            // Act
            
            var result = statePolicy.Invoke(mockContext.Object, input);
            // Assert
            result.Success.Should().Be(true);
            result.Policy.Should().Be(statePolicy);
            result.Value.Should().Be(input);
            result.ChildResults.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void TestRetentionStateReturnDefaultNotSuccessOnNotFound()
        {
            // Arrange
            var input = new object();
            var state = "SomeState";
            
            var mockContext = new Mock<IRetentionContext<object>>();
            var mockPolicyManager = new Mock<IRetentionPolicyManager<object>>();

            mockPolicyManager.Setup(m => m.Get(
                    It.IsAny<object>(),
                    It.IsAny<object?>()))
                .Returns((IRetentionPolicy<object>?) null);
            
            mockContext.Setup(c => c.PolicyManager)
                .Returns(mockPolicyManager.Object);
            
            var statePolicy = new RetentionStatePolicy<object>(state, false);
            // Act
            
            var result = statePolicy.Invoke(mockContext.Object, input);
            // Assert
            result.Success.Should().Be(false);
            result.Policy.Should().Be(statePolicy);
            result.Value.Should().Be(input);
            result.ChildResults.Should().BeNullOrEmpty();
        }
    }
}