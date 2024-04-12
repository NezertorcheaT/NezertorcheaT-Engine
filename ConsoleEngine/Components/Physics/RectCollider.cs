using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public class RectCollider : Collider, IPolygonamical, IAabb
    {
        public double[] Size;

        IEnumerable<Collision> IAabb.Check()
        {
            //throw new System.NotImplementedException();
            var thisBox = Bounds;
            var boxes = CollidersCounter.AABBs.ToArray();
            foreach (var box in boxes)
            {
                if (box == thisBox) continue;
                var diff = Bounds.MinkowskiDif(box, thisBox);
                //Logger.Log($"{diff} {diff.Contains(new Vector2(0, 0))} Vector2(0, 0)");

                if (!diff.Contains(new Vector2(0, 0))) continue;
                var collision = new Collision();
                Vector2[] faces =
                {
                    new Vector2(-1, 0),
                    new Vector2(1, 0),
                    new Vector2(0, -1),
                    new Vector2(0, 1)
                };

                var maxA = thisBox.Position + thisBox.Extends;
                var minA = thisBox.Position - thisBox.Extends;
                var maxB = box.Position + box.Extends;
                var minB = box.Position - box.Extends;

                float[] distances =
                {
                    maxB.X - minA.X,
                    maxA.X - minB.X,
                    maxB.Y - minA.Y,
                    maxA.Y - minB.Y,
                };

                collision.Distance = float.MaxValue;

                for (var i = 0; i < 4; i++)
                {
                    if (distances[i] < collision.Distance)
                    {
                        collision.Distance = distances[i];
                        collision.Normal = -faces[i];
                    }
                }

                yield return collision;
            }
        }

        IEnumerable<Collision> IPolygonamical.Check()
        {
            var thisSats = ToSatTriangles.ToArray();
            var triangles = CollidersCounter.Triangles.ToArray();
            foreach (var triangle in triangles)
            {
                foreach (var thisSat in thisSats)
                {
                    //Logger.Log(triangle, "triangle");
                    if (triangle == thisSat) continue;
                    var collision = thisSat.CheckCollision(triangle) ?? triangle.CheckCollision(thisSat);
                    collision ??= triangle.CheckCollision(thisSat) ?? thisSat.CheckCollision(triangle);

                    if (collision is not null) yield return collision.Value;
                }
            }
        }

        public IEnumerable<SatTriangle> ToSatTriangles
        {
            get
            {
                yield return new SatTriangle(Bounds.LeftUp, Bounds.LeftDown, Bounds.RightUp);
                yield return new SatTriangle(Bounds.RightUp, Bounds.RightDown, Bounds.LeftDown);
            }
        }

        public Bounds Bounds => new Bounds(Size.Da2V2(), transform.Position);
    }
}