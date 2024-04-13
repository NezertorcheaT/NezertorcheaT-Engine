using System.Collections.Generic;
using Engine.Core;

namespace Engine.Components.Physics
{
    public interface IAabb : IComponentInit
    {
        Bounds Bounds { get; }
        IEnumerable<Collision> Check();
    }
}