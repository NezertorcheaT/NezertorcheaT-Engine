using System;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components
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
        public Matrix3x2 RotationMatrix => Matrix3x2.CreateRotation(GetRotation(RotationRadians, Parent));

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

                loc += Vector2.Transform(par.LocalPosition, par.RotationMatrix);
                //loc += par.LocalPosition;
                par = par.Parent;
            }
        }
    }
}