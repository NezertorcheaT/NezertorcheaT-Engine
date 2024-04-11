using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public struct SatTriangle
    {
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        public IEnumerable<Vector2> Verts
        {
            get
            {
                yield return P1;
                yield return P2;
                yield return P3;
            }
        }

        public IEnumerable<Vector2> VertNormals
        {
            get
            {
                yield return Vector2.Normalize(P1 - Centroid);
                yield return Vector2.Normalize(P2 - Centroid);
                yield return Vector2.Normalize(P3 - Centroid);
            }
        }

        public IEnumerable<Vector2> EdgeNormals
        {
            get
            {
                yield return Helper.Normal(P1, P2);
                yield return Helper.Normal(P2, P3);
                yield return Helper.Normal(P3, P1);
            }
        }

        public Vector2 Centroid => Verts.Aggregate(new Vector2(), (current, vert) => current + vert) / 3f;


        public SatTriangle(Vector2 P1, Vector2 P2, Vector2 P3)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.P3 = P3;
        }

        public static bool operator ==(SatTriangle a, SatTriangle b) => a.Equals(b);
        public static bool operator !=(SatTriangle a, SatTriangle b) => !a.Equals(b);

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            var t = obj is SatTriangle ? (SatTriangle) obj : (SatTriangle?) null;
            if (t is null) return false;
            return P1 == t.Value.P1 && P2 == t.Value.P2 && P3 == t.Value.P3;
        }

        public static Collision? CheckCollision(SatTriangle triangle1, SatTriangle triangle2)
        {
            throw new NotImplementedException("TODO GJK collision");
            
            var collision = new Collision();
            var vertecies1 = triangle1.Verts.ToArray();
            var vertecies2 = triangle2.Verts.ToArray();
            var normals1 = triangle1.VertNormals.ToArray();
            var normals2 = triangle2.VertNormals.ToArray();
            var separation = float.MinValue;

            for (var va = 0; va < 3; va++)
            {
                var normal = normals1[va];
                var minimumSeparation = float.MaxValue;
                foreach (var vert1 in vertecies2)
                {
                    var proj = Vector2.Dot(vert1 - vertecies1[va], normal);
                    minimumSeparation = MathF.Min(minimumSeparation, proj);
                }

                if (minimumSeparation > separation)
                {
                    separation = minimumSeparation;
                    collision.Normal = normal;
                    collision.Distance = separation;
                }
            }

            //Logger.Log(separation, "sat separation");

            //return collision;
            if (separation <= 0) return collision;

            return null;
        }

        public override string ToString() => $"SatTriangle({P1}, {P2}, {P3})";
    }
}