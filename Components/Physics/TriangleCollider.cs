using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TriangleCollider : Collider, IPolygonamical
    {
        public double[] P1;
        public double[] P2;
        public double[] P3;

        IEnumerable<SatTriangle> IPolygonamical.ToSatTriangles => new[] {SatTriangle};

        private SatTriangle SatTriangle => new SatTriangle(
            Vector2.Transform(P1.Da2V2(), transform.LocalRotationMatrix) + transform.Position,
            Vector2.Transform(P2.Da2V2(), transform.LocalRotationMatrix) + transform.Position,
            Vector2.Transform(P3.Da2V2(), transform.LocalRotationMatrix) + transform.Position
        );

        protected override Collision? Check()
        {
            var thisSat = SatTriangle;
            var triangles = CollidersCounter.Triangles.ToList();
            foreach (var triangle in triangles)
            {
                //Logger.Log(triangle, "triangle");
                if (triangle.Equals(thisSat)) continue;
                var collision = thisSat.CheckCollision(triangle);

                if (collision is null) continue;
                return collision;
            }

            return null;
        }
    }
}