using System.Collections.Generic;

namespace ConsoleEngine.Components.Physics
{
    public interface IPolygonamical:IComponentInit
    {
        IEnumerable<SatTriangle> ToSatTriangles { get; }
    }
}