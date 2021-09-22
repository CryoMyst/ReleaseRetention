using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Retention.Abstractions;
using Retention.Policies.Logical;
using Xunit;

namespace Retention.Tests.Policies
{
    /// <summary>
    ///     Used to test the <see cref="RetentionOrPolicy{T}"/>
    /// </summary>
    public class RetentionOrPolicyUnitTests
    {
        [Theory]
        [InlineData(new bool[] { true }, true)]
        [InlineData(new bool[] { false }, false)]
        [InlineData(new bool[] { true, true }, true)]
        [InlineData(new bool[] { true, false }, true)]
        [InlineData(new bool[] { false, false }, false)]
        public void TestOrPolicyTruthTable(bool[] inputs, bool expected)
        {
            // Arrange
            var value = new object();

            // Mocking the retention context
            var mockContext = new Mock<IRetentionContext<object>>();
            
            // Mocking the input policies
            var inputPolicies = inputs.Select(input =>
            {
                var mock = new Mock<IRetentionPolicy<object>>();
                mock.Setup(m => m.Invoke(mockContext.Object, value))
                    .Returns(new RetentionResult<object>(value, input, mock.Object));
                return mock.Object;
            }).ToArray();


            var orPolicy = new RetentionOrPolicy<object>(inputPolicies);
            
            // Act
            var result = orPolicy.Invoke(mockContext.Object, value);

            // Assert
            result.Success.Should().Be(expected);
        }

        [Fact]
        public void TestOrPolicyThrowsOnEmptyInitialize()
        {
            // Arrage
            // Act
            // Assert
            FluentActions.Invoking(() => new RetentionOrPolicy<object>())
                .Should()
                .Throw<ArgumentException>();
        }
    }
}