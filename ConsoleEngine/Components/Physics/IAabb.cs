using System.Collections.Generic;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public interface IAabb : IComponentInit
    {
        Bounds Bounds { get; }
        Collision? Check();
    }
}