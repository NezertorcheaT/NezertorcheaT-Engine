using System;
using System.Collections;
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

        public static Collision? CheckCollision(SatTriangle triangle1, SatTriangle triangle2)
        {
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
                for (var vb = 0; vb < 3; vb++)
                {
                    var proj = Vector2.Dot(vertecies1[vb] - vertecies2[va], normal);
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

            if (separation >= 0) return collision;

            return null;
        }

        public override string ToString()
        {
            return $"SatTriangle({P1}, {P2}, {P3})";
        }
    }
}