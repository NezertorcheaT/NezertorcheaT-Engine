using System.Collections.Generic;
using System.Linq;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public class CollidersCounter : Component, IComponentUpdate
    {
        public static IEnumerable<SatTriangle> Triangles
        {
            get
            {
                if (_triangles is null)
                {
                    UpdateTriangles();
                }

                return _triangles;
            }
        }

        private static List<SatTriangle> _triangles;

        private static void UpdateTriangles()
        {
            _triangles = new List<SatTriangle>();
            if (GameConfig.GameHierarchy is null) return;
            if (GameConfig.GameHierarchy.Objects.Count == 0) return;

            foreach (var collider in GameConfig.GameHierarchy.Objects.First().GetAllComponents<Collider>())
            {
                if (!(collider is IPolygonamical pol)) continue;
                _triangles.AddRange(pol.ToSatTriangles);
            }
        }

        void IComponentUpdate.Update() => UpdateTriangles();
    }
}