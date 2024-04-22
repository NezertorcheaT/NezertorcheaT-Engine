using System.Collections.Generic;

namespace Engine.Scene
{
    internal interface ISetHierarchyName
    {
        string MapName { get; set; }
    }

    public class Hierarchy : ISetHierarchyName
    {
        public List<GameObject> Objects { get; }

        public Hierarchy()
        {
            Objects = new List<GameObject>();
        }

        string ISetHierarchyName.MapName
        {
            get => MapName;
            set => MapName = value;
        }

        public string MapName { get; private set; }
    }
}