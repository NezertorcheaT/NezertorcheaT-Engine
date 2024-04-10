using System.Collections.Generic;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public interface IPolygonamical:IComponentInit
    {
        IEnumerable<SatTriangle> ToSatTriangles { get; }
        Bounds Bounds { get; }
    }
}