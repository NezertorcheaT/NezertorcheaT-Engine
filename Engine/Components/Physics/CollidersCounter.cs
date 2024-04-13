using System.Collections.Generic;
using Engine.Core;
using Engine.Scene;

namespace Engine.Components.Physics
{
    public class CollidersCounter : Component
    {
        public static IEnumerable<SatTriangle> Triangles
        {
            get
            {
                if (GameConfig.GameHierarchy is null) yield break;
                if (GameConfig.GameHierarchy.Objects.Count == 0) yield break;

                foreach (var collider in GameObject.FindAllTypes<IPolygonamical>(GameConfig.GameHierarchy))
                {
                    if (!(collider is IPolygonamical pol)) continue;
                    foreach (var triangle in pol.ToSatTriangles)
                        yield return triangle;
                }
            }
        }
        public static IEnumerable<Bounds> AABBs
        {
            get
            {
                if (GameConfig.GameHierarchy is null) yield break;
                if (GameConfig.GameHierarchy.Objects.Count == 0) yield break;

                foreach (var collider in GameObject.FindAllTypes<IAabb>(GameConfig.GameHierarchy))
                {
                    yield return (collider as IAabb)?.Bounds;
                }
            }
        }
    }
}