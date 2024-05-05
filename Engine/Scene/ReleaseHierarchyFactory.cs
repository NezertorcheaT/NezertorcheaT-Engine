namespace Engine.Scene
{
    public class ReleaseHierarchyFactory : IHierarchyFactory
    {
        string IHierarchyFactory.SaveHierarchy(Hierarchy hierarchy)
        {
            return new DevHierarchyFactory().SaveHierarchy(hierarchy);
        }

        Hierarchy IHierarchyFactory.CreateHierarchy(string content, string mapName, bool debug)
        {
            return new DevHierarchyFactory().CreateHierarchy(content, mapName, debug);
        }
    }
}