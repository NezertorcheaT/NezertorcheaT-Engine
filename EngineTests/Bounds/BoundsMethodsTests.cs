using System.Numerics;
using FluentAssertions;
using Xunit;

namespace EngineTests.Bounds
{
    public class BoundsMethodsTests
    {
        [Fact]
        public void Bounds_Equals_Boolean()
        {
            //Arrange
            var bounds1 = new Engine.Core.Bounds(new Vector2(2, 2));
            var bounds2 = new Engine.Core.Bounds(new Vector2(2, 2), new Vector2(0, 0));
            var bounds3 = new Engine.Core.Bounds(new[] {new Vector2(-1, -1), new Vector2(1, 1)});

            //Act
            var result1 = bounds1 == bounds2;
            var result2 = bounds2 == bounds3;
            var result3 = bounds3 == bounds1;

            //Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
        }

        [Fact]
        public void Bounds_Contains_Bounds()
        {
            //Arrange
            var bounds1 = new Engine.Core.Bounds(new Vector2(2, 2));
            var bounds2 = new Engine.Core.Bounds(new Vector2(2, 2), new Vector2(0, 0));
            var bounds3 = new Engine.Core.Bounds(new[] {new Vector2(-1, -1), new Vector2(1, 1)});
            var pos1 = new Vector2(0, 0);
            var pos2 = new Vector2(5, -5);
            var pos3 = new Vector2(1, -1);

            //Act
            var result1 = bounds1.Contains(pos1);
            var result2 = bounds2.Contains(pos2);
            var result3 = bounds3.Contains(pos3);

            //Assert
            result1.Should().BeTrue();
            result2.Should().BeFalse();
            result3.Should().BeTrue();
        }

        [Fact]
        public void Bounds_Clamp_Vector2()
        {
            //Arrange
            var bounds = new Engine.Core.Bounds(new Vector2(2, 2));
            var pos1 = new Vector2(0, 0);
            var pos2 = new Vector2(5, -5);
            var pos3 = new Vector2(1, 1);

            //Act
            var result1 = bounds.Clamp(pos1);
            var result2 = bounds.Clamp(pos2);
            var result3 = bounds.Clamp(pos3);

            //Assert
            result1.Should().BeEquivalentTo(new Vector2(0, 0));
            result2.Should().BeEquivalentTo(new Vector2(1, -1));
            result3.Should().BeEquivalentTo(new Vector2(1, 1));
        }
    }
}