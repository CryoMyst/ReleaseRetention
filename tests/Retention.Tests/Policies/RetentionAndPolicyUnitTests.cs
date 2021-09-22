using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Retention.Abstractions;
using Retention.Policies.Logical;
using Xunit;

namespace Retention.Tests.Policies
{
    /// <summary>
    ///     Used to test the <see cref="RetentionAndPolicy{T}"/>
    /// </summary>
    public class RetentionAndPolicyUnitTests
    {
        [Theory]
        [InlineData(new bool[] { true }, true)]
        [InlineData(new bool[] { false }, false)]
        [InlineData(new bool[] { true, true }, true)]
        [InlineData(new bool[] { true, false }, false)]
        [InlineData(new bool[] { false, true }, false)]
        [InlineData(new bool[] { false, false }, false)]
        [InlineData(new bool[] { true, true, false }, false)]
        [InlineData(new bool[] { true, true, true }, true)]
        public void TestAndPolicyTruthTable(bool[] inputs, bool expected)
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


            var andPolicy = new RetentionAndPolicy<object>(inputPolicies);
            
            // Act
            var result = andPolicy.Invoke(mockContext.Object, value);

            // Assert
            result.Success.Should().Be(expected);
        }

        [Fact]
        public void TestAndPolicyThrowsOnEmptyInitialize()
        {
            // Arrage
            // Act
            // Assert
            FluentActions.Invoking(() => new RetentionAndPolicy<object>())
                .Should()
                .Throw<ArgumentException>();
        }
    }
}