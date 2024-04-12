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
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        IEnumerable<SatTriangle> IPolygonamical.ToSatTriangles => new[] {SatTriangle};
        public Bounds Bounds => new Bounds(SatTriangle.Verts);

        private SatTriangle SatTriangle => new SatTriangle(
            P1 + transform.Position,
            P2 + transform.Position,
            P3 + transform.Position
        );

        IEnumerable<Collision> IPolygonamical.Check()
        {
            var thisSat = SatTriangle;
            var triangles = CollidersCounter.Triangles.ToArray();
            foreach (var triangle in triangles)
            {
                //Logger.Log(triangle, "triangle");
                if (triangle == thisSat) continue;
                var collision = thisSat.CheckCollision(triangle) ?? triangle.CheckCollision(thisSat);
                collision ??= triangle.CheckCollision(thisSat) ?? thisSat.CheckCollision(triangle);

                if (collision is not null) yield return collision.Value;
            }
        }
    }
}