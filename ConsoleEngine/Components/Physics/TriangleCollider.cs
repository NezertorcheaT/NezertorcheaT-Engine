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
        public Bounds Bounds => new Bounds(SatTriangle.Verts);

        private SatTriangle SatTriangle => new SatTriangle(
            P1.Da2V2() + transform.Position,
            P2.Da2V2() + transform.Position,
            P3.Da2V2() + transform.Position
        );

        Collision? IPolygonamical.Check()
        {
            var thisSat = SatTriangle;
            var triangles = CollidersCounter.Triangles.ToArray();
            foreach (var triangle in triangles)
            {
                //Logger.Log(triangle, "triangle");
                if (triangle == thisSat) continue;
                var collision = thisSat.CheckCollision(triangle) ?? triangle.CheckCollision(thisSat);
                collision ??= triangle.CheckCollision(thisSat) ?? thisSat.CheckCollision(triangle);

                if (collision is not null) return collision;
            }

            return null;
        }
    }
}