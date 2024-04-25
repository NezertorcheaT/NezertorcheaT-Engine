using System.Numerics;
using Engine.Core;

namespace Engine.Components
{
    public class Transform : Component
    {
        public Vector2 LocalPosition;

        public float LocalRotationRadians => LocalRotation * Helper.DegToRadFloat;

        public float LocalRotation;

        public Transform? Parent;

        public Vector2 Position => GetPosition(LocalPosition, Parent);
        public float Rotation => GetRotation(LocalRotationRadians, Parent);
        public float RotationRadians => Rotation * Helper.DegToRadFloat;
        public Matrix3x2 RotationMatrix => Matrix3x2.CreateRotation(RotationRadians);
        public Matrix3x2 LocalRotationMatrix => Matrix3x2.CreateRotation(LocalRotationRadians);

        private static float GetRotation(float rot, Transform? par)
        {
            while (true)
            {
                if (par == null) return rot;

                rot += par.LocalRotation;
                par = par.Parent;
            }
        }

        private static Vector2 GetPosition(Vector2 loc, Transform? par)
        {
            while (true)
            {
                if (par == null) return loc;

                loc = Vector2.Transform(loc, par.LocalRotationMatrix) + par.LocalPosition;
                par = par.Parent;
            }
        }
    }
}