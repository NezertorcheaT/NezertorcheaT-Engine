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
                if (GameConfig.SceneManager.CurrentHierarchy is null) yield break;
                if (GameConfig.SceneManager.CurrentHierarchy.Objects.Count == 0) yield break;

                foreach (var collider in GameObject.FindAllTypes<IPolygonamical>(GameConfig.SceneManager.CurrentHierarchy))
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
                if (GameConfig.SceneManager.CurrentHierarchy is null) yield break;
                if (GameConfig.SceneManager.CurrentHierarchy.Objects.Count == 0) yield break;

                foreach (var collider in GameObject.FindAllTypes<IAabb>(GameConfig.SceneManager.CurrentHierarchy))
                {
                    yield return (collider as IAabb)?.Bounds;
                }
            }
        }
    }
}