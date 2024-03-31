using System.Collections.Generic;

namespace ConsoleEngine.Components.Physics
{
    public interface IPolygonamical
    {
        IEnumerable<SatTriangle> ToSatTriangles { get; }
    }
}