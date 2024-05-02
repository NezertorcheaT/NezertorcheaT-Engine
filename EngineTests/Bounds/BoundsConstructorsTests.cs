using System.Numerics;
using FluentAssertions;
using Xunit;

namespace EngineTests.Bounds
{
    public class BoundsConstructorsTests
    {
        [Fact]
        public void Bounds_SizeConstructor_Bounds()
        {
            //Arrange
            Engine.Core.Bounds bounds;

            //Act
            bounds = new Engine.Core.Bounds(new Vector2(2, 2));

            //Assert
            bounds.Position.Should().BeEquivalentTo(Vector2.Zero);
            bounds.Size.Should().BeEquivalentTo(new Vector2(2, 2));
        }

        [Fact]
        public void Bounds_SizePosConstructor_Bounds()
        {
            //Arrange
            Engine.Core.Bounds bounds;

            //Act
            bounds = new Engine.Core.Bounds(new Vector2(2, 2), new Vector2(2, 2));

            //Assert
            bounds.Position.Should().BeEquivalentTo(new Vector2(2, 2));
            bounds.Size.Should().BeEquivalentTo(new Vector2(2, 2));
        }

        [Fact]
        public void Bounds_PointsConstructor_Bounds()
        {
            //Arrange
            Engine.Core.Bounds bounds;

            //Act
            bounds = new Engine.Core.Bounds(new[] {new Vector2(-1, -1), new Vector2(1, 1)});

            //Assert
            bounds.Position.Should().BeEquivalentTo(Vector2.Zero);
            bounds.Size.Should().BeEquivalentTo(new Vector2(2, 2));
        }

        [Fact]
        public void Bounds_EmptyConstructor_Bounds()
        {
            //Arrange
            Engine.Core.Bounds bounds;

            //Act
            bounds = new Engine.Core.Bounds();

            //Assert
            bounds.Position.Should().BeEquivalentTo(Vector2.Zero);
            bounds.Size.Should().BeEquivalentTo(Vector2.Zero);
        }
    }
}