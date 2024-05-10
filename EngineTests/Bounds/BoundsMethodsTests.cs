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
        public void Bounds_Contains_Bool()
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
        public void Bounds_PropertiesGet_Float()
        {
            //Arrange
            var bounds1 = new Engine.Core.Bounds(new Vector2(2, 2));
            var bounds2 = new Engine.Core.Bounds(new[] {new Vector2(-1, -1), new Vector2(1, 1)});

            //Act
            var resultDown1 = bounds1.Down;
            var resultDown2 = bounds2.Down;

            var resultUp1 = bounds1.Up;
            var resultUp2 = bounds2.Up;

            var resultRight1 = bounds1.Right;
            var resultRight2 = bounds2.Right;

            var resultLeft1 = bounds1.Down;
            var resultLeft2 = bounds2.Left;

            //Assert
            resultDown1.Should().Be(-1);
            resultDown2.Should().Be(-1);
            resultUp1.Should().Be(1);
            resultUp2.Should().Be(1);
            resultRight1.Should().Be(1);
            resultRight2.Should().Be(1);
            resultLeft1.Should().Be(-1);
            resultLeft2.Should().Be(-1);
        }

        [Fact]
        public void Bounds_PropertiesGet_Vector2()
        {
            //Arrange
            var bounds1 = new Engine.Core.Bounds(new Vector2(2, 2));
            var bounds2 = new Engine.Core.Bounds(new[] {new Vector2(-1, -1), new Vector2(1, 1)});

            //Act
            var resultExtends1 = bounds1.Extends;
            var resultExtends2 = bounds2.Extends;

            var resultLeftDown1 = bounds1.LeftDown;
            var resultLeftDown2 = bounds2.LeftDown;

            var resultRightDown1 = bounds1.RightDown;
            var resultRightDown2 = bounds2.RightDown;

            var resultLeftUp1 = bounds1.LeftUp;
            var resultLeftUp2 = bounds2.LeftUp;

            var resultRightUp1 = bounds1.RightUp;
            var resultRightUp2 = bounds2.RightUp;

            //Assert
            resultExtends1.Should().BeEquivalentTo(new Vector2(1, 1));
            resultExtends2.Should().BeEquivalentTo(new Vector2(1, 1));
            resultLeftDown1.Should().BeEquivalentTo(new Vector2(-1, -1));
            resultLeftDown2.Should().BeEquivalentTo(new Vector2(-1, -1));
            resultRightDown1.Should().BeEquivalentTo(new Vector2(1, -1));
            resultRightDown2.Should().BeEquivalentTo(new Vector2(1, -1));
            resultLeftUp1.Should().BeEquivalentTo(new Vector2(-1, 1));
            resultLeftUp2.Should().BeEquivalentTo(new Vector2(-1, 1));
            resultRightUp1.Should().BeEquivalentTo(new Vector2(1, 1));
            resultRightUp2.Should().BeEquivalentTo(new Vector2(1, 1));
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