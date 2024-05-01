namespace Engine.Scene
{
    internal interface IHierarchyFactory
    {
        string SaveHierarchy(Hierarchy hierarchy);
        Hierarchy CreateHierarchy(string path, bool debug = true);
    }
}