﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Engine.Core
{
    public class Bounds
    {
        public Vector2 Size;
        public Vector2 Position;

        public Vector2 Extends
        {
            get => (Size / 2f).Abs();
            set => Size = value.Abs() * 2f;
        }

        public float Right
        {
            get => Position.X + Extends.X;
            set => UpdateBounds(value, Left, Up, Down);
        }

        public float Left
        {
            get => Position.X - Extends.X;
            set => UpdateBounds(Right, value, Up, Down);
        }

        public float Down
        {
            get => Position.Y - Extends.Y;
            set => UpdateBounds(Right, Left, Up, value);
        }

        public float Up
        {
            get => Position.Y + Extends.Y;
            set => UpdateBounds(Right, Left, value, Down);
        }

        public Vector2 RightUp => new Vector2(Right, Up);
        public Vector2 LeftUp => new Vector2(Left, Up);
        public Vector2 RightDown => new Vector2(Right, Down);
        public Vector2 LeftDown => new Vector2(Left, Down);

        public Bounds()
        {
            Size = new Vector2();
            Position = new Vector2();
        }

        public Bounds(Vector2 size)
        {
            Size = size.Abs();
            Position = new Vector2();
        }

        public Bounds(Vector2 size, Vector2 position)
        {
            Size = size.Abs();
            Position = position;
        }

        public Bounds(IEnumerable<Vector2> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));

            var pointsArray = points.ToArray();
            if (pointsArray.Length <= 1)
                throw new ArgumentException("Minimum of points.Length must be 2", nameof(points));

            var left = float.MaxValue;
            var right = float.MinValue;
            var up = float.MinValue;
            var down = float.MaxValue;

            foreach (var point in pointsArray)
            {
                right = MathF.Max(point.X, right);
                left = MathF.Min(point.X, left);
                up = MathF.Max(point.Y, up);
                down = MathF.Min(point.Y, down);
            }

            UpdateBounds(right, left, up, down);
        }

        public bool Contains(Vector2 point) => point.X >= Left && point.X <= Right && point.Y >= Down && point.Y <= Up;

        public Vector2 Clamp(Vector2 point) =>
            new Vector2(Math.Clamp(point.X, Left, Right), Math.Clamp(point.Y, Down, Up));

        public static Bounds MinkowskiSum(Bounds a, Bounds b)
        {
            return new Bounds(new[]
                {
                    a.LeftUp + b.LeftUp,
                    a.LeftUp + b.RightUp,
                    a.LeftUp + b.LeftDown,
                    a.LeftUp + b.RightDown,
                    a.RightUp + b.LeftUp,
                    a.RightUp + b.RightUp,
                    a.RightUp + b.LeftDown,
                    a.RightUp + b.RightDown,
                    a.LeftDown + b.LeftUp,
                    a.LeftDown + b.RightUp,
                    a.LeftDown + b.LeftDown,
                    a.LeftDown + b.RightDown,
                    a.RightDown + b.LeftUp,
                    a.RightDown + b.RightUp,
                    a.RightDown + b.LeftDown,
                    a.RightDown + b.RightDown,
                }
            );
        }

        public static Bounds MinkowskiDif(Bounds a, Bounds b)
        {
            return new Bounds(new[]
                {
                    a.LeftUp - b.LeftUp,
                    a.LeftUp - b.RightUp,
                    a.LeftUp - b.LeftDown,
                    a.LeftUp - b.RightDown,
                    a.RightUp - b.LeftUp,
                    a.RightUp - b.RightUp,
                    a.RightUp - b.LeftDown,
                    a.RightUp - b.RightDown,
                    a.LeftDown - b.LeftUp,
                    a.LeftDown - b.RightUp,
                    a.LeftDown - b.LeftDown,
                    a.LeftDown - b.RightDown,
                    a.RightDown - b.LeftUp,
                    a.RightDown - b.RightUp,
                    a.RightDown - b.LeftDown,
                    a.RightDown - b.RightDown,
                }
            );
        }

        private void UpdateBounds(float right, float left, float up, float down)
        {
            Position = (
                new Vector2(right, up) +
                new Vector2(left, up) +
                new Vector2(right, down) +
                new Vector2(left, down)
            ) / 4f;
            Size = new Vector2(right - left, up - down).Abs();
        }

        public static bool operator ==(Bounds a, Bounds b) => a.Equals(b);
        public static bool operator !=(Bounds a, Bounds b) => !a.Equals(b);

        protected bool Equals(Bounds other)
        {
            return Size.Equals(other.Size) && Position.Equals(other.Position);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Bounds) obj);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Size, Position);
        }

        public override string ToString() => $"Bounds(Position: {Position}, Size: {Size})";
    }
}