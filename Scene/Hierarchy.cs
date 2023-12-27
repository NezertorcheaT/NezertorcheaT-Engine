using System.Collections.Generic;

namespace ConsoleEngine.Scene
{
    public class Hierarchy
    {
        public List<GameObject> Objs { get; }

        public Hierarchy()
        {
            Objs = new List<GameObject>();
        }
    }
}