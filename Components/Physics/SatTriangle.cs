using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public struct SatTriangle
    {
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        public IEnumerable<Vector2> normals
        {
            get
            {
                yield return Helper.Normal(P1, P2);
                yield return Helper.Normal(P2, P3);
                yield return Helper.Normal(P3, P1);
            }
        }

        public SatTriangle(Vector2 P1, Vector2 P2, Vector2 P3)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.P3 = P3;
        }

        public static Collision? CheckCollision(SatTriangle triangle1, SatTriangle triangle2)
        {
            var collision = new Collision();

            foreach (var normal in triangle1.normals)
            {
            }

            return collision;
        }
    }
}