using System.Numerics;

namespace ConsoleEngine.Components.Physics
{
    public struct Collision
    {
        public Vector2 Normal;
        public float Distance;

        public override string ToString() => $"Collision({Normal}, {Distance})";

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            var t = obj is Collision ? (Collision) obj : (Collision?) null;
            if (t is null) return false;
            return Normal == t.Value.Normal && Distance == t.Value.Distance;
        }

        public static bool operator ==(Collision a, Collision b) => a.Equals(b);
        public static bool operator !=(Collision a, Collision b) => !(a == b);
    }
}