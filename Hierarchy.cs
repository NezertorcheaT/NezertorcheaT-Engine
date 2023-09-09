using System.Collections.Generic;

namespace Engine
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