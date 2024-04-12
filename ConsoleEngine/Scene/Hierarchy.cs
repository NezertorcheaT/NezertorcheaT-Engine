using System.Collections.Generic;

namespace ConsoleEngine.Scene
{
    public class Hierarchy
    {
        public List<GameObject> Objects { get; }

        public Hierarchy()
        {
            Objects = new List<GameObject>();
        }
    }
}