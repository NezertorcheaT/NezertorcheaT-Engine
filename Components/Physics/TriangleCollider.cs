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

        public Bounds Bounds
        {
            get
            {
                var b = new Bounds();
                var polygonamical = this as IPolygonamical;
                var n = 0;
                
                foreach (var triangle in polygonamical.ToSatTriangles)
                {
                    b.Position += triangle.Centroid;
                    n++;
                }
                b.Position /= n;
                
                
                foreach (var triangle in polygonamical.ToSatTriangles)
                {
                    foreach (var vert in triangle.Verts)
                    {
                        
                    }
                }

                return b;
            }
        }

        private SatTriangle SatTriangle => new SatTriangle(
            P1.Da2V2() + transform.Position,
            P2.Da2V2() + transform.Position,
            P3.Da2V2() + transform.Position
        );

        protected override Collision? Check()
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