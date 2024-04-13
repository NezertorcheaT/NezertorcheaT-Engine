using System.Collections.Generic;
using Engine.Core;

namespace Engine.Components.Physics
{
    public interface IPolygonamical : IComponentInit
    {
        IEnumerable<SatTriangle> ToSatTriangles { get; }
        Bounds Bounds { get; }
        IEnumerable<Collision> Check();
    }
}
