namespace Engine.Scene
{
    internal interface IHierarchyFactory
    {
        string SaveHierarchy(Hierarchy hierarchy);
        Hierarchy CreateHierarchy(string content, string mapName, bool debug = true);
    }
}