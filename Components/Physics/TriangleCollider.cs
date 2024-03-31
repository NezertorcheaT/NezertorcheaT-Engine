using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TriangleCollider : Collider, IPolygonamical
    {
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        IEnumerable<SatTriangle> IPolygonamical.ToSatTriangles => new[] {SatTriangle};

        private SatTriangle SatTriangle => new SatTriangle(
            Vector2.Transform(P1 + transform.Position, transform.LocalRotationMatrix),
            Vector2.Transform(P2 + transform.Position, transform.LocalRotationMatrix),
            Vector2.Transform(P3 + transform.Position, transform.LocalRotationMatrix)
        );

        protected override Collision? Check()
        {
            var thisSat = SatTriangle;
            foreach (var triangle in CollidersCounter.Triangles)
            {
                if (triangle.Equals(thisSat)) continue;
                var collision = thisSat.CheckCollision(triangle);

                if (collision is null) continue;
                return collision;
            }

            return null;
        }
    }
}