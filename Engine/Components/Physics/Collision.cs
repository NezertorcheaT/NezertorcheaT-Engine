using System;
using System.Numerics;

namespace Engine.Components.Physics
{
    public struct Collision
    {
        public Vector2 Normal;
        public float Distance;

        public override string ToString() => $"Collision({Normal}, {Distance})";

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Collision) obj);
        }

        public static bool operator ==(Collision a, Collision b) => a.Equals(b);
        public static bool operator !=(Collision a, Collision b) => !(a == b);

        private bool Equals(Collision other)
        {
            return Normal.Equals(other.Normal) && Distance.Equals(other.Distance);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Normal, Distance);
        }

    }
}