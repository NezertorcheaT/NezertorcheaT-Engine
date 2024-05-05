using System.Text.Json;

namespace Engine.Scene
{
    public class ReleaseHierarchyFactory : IHierarchyFactory
    {
        private static readonly string JsonGameObjectNameLiteral = "name";
        private static readonly string JsonGameObjectTagLiteral = "tag";
        private static readonly string JsonGameObjectLayerLiteral = "layer";
        private static readonly string JsonGameObjectActiveLiteral = "active";
        private static readonly string JsonComponentsLiteral = "components";
        private static readonly string JsonComponentDataLiteral = "data";
        private static readonly string JsonComponentDataValueLiteral = "value";
        private static readonly string JsonComponentNameLiteral = "name";
        private static readonly string JsonComponentDataNameLiteral = "name";
        private static readonly string JsonComponentEnabledLiteral = "enabled";
        private static readonly string JsonNullLiteral = "null";

        private static readonly string GameObjectNameLiteral = "ě";
        private static readonly string GameObjectTagLiteral = "đ";
        private static readonly string GameObjectLayerLiteral = "Ē";
        private static readonly string GameObjectActiveLiteral = "ē";
        private static readonly string ComponentsLiteral = "Ĕ";
        private static readonly string ComponentDataLiteral = "ĕ";
        private static readonly string ComponentDataValueLiteral = "Ė";
        private static readonly string ComponentNameLiteral = "ė";
        private static readonly string ComponentDataNameLiteral = "Ę";
        private static readonly string ComponentEnabledLiteral = "ę";
        private static readonly string NullLiteral = "Ě";

        private static readonly char CurvBracOpen = 'Ĩ';
        private static readonly char CurvBracClose = 'ĩ';
        private static readonly char SqBracOpen = 'Ī';
        private static readonly char SqBracClose = 'ī';
        private static readonly char Coma = 'Ĭ';
        private static readonly char Cvots = 'ĭ';

        string IHierarchyFactory.SaveHierarchy(Hierarchy hierarchy)
        {
            var j = JsonSerializer.SerializeToNode(new DevHierarchyFactory().SaveHierarchy(hierarchy)).ToString();

            var inCvots = false;
            foreach (var symb in j)
            {
                if (symb == '"') inCvots = !inCvots;
                if (inCvots) continue;
                j = j.Replace(JsonGameObjectNameLiteral, GameObjectNameLiteral);
                j = j.Replace(JsonGameObjectTagLiteral, GameObjectTagLiteral);
                j = j.Replace(JsonGameObjectLayerLiteral, GameObjectLayerLiteral);
                j = j.Replace(JsonGameObjectActiveLiteral, GameObjectActiveLiteral);
                j = j.Replace(JsonComponentsLiteral, ComponentsLiteral);
                j = j.Replace(JsonComponentDataLiteral, ComponentDataLiteral);
                j = j.Replace(JsonComponentDataValueLiteral, ComponentDataValueLiteral);
                j = j.Replace(JsonComponentNameLiteral, ComponentNameLiteral);
                j = j.Replace(JsonComponentDataNameLiteral, ComponentDataNameLiteral);
                j = j.Replace(JsonComponentEnabledLiteral, ComponentEnabledLiteral);
                j = j.Replace(JsonNullLiteral, NullLiteral);
                j = j.Replace('{', CurvBracOpen);
                j = j.Replace('}', CurvBracClose);
                j = j.Replace('[', SqBracOpen);
                j = j.Replace(']', SqBracClose);
                j = j.Replace(',', Coma);
                j = j.Replace('"', Cvots);
            }

            return j;
        }

        Hierarchy IHierarchyFactory.CreateHierarchy(string content, string mapName, bool debug)
        {
            content = content.Replace(GameObjectNameLiteral, JsonGameObjectNameLiteral);
            content = content.Replace(GameObjectTagLiteral, JsonGameObjectTagLiteral);
            content = content.Replace(GameObjectLayerLiteral, JsonGameObjectLayerLiteral);
            content = content.Replace(GameObjectActiveLiteral, JsonGameObjectActiveLiteral);
            content = content.Replace(ComponentsLiteral, JsonComponentsLiteral);
            content = content.Replace(ComponentDataLiteral, JsonComponentDataLiteral);
            content = content.Replace(ComponentDataValueLiteral, JsonComponentDataValueLiteral);
            content = content.Replace(ComponentNameLiteral, JsonComponentNameLiteral);
            content = content.Replace(ComponentDataNameLiteral, JsonComponentDataNameLiteral);
            content = content.Replace(ComponentEnabledLiteral, JsonComponentEnabledLiteral);
            content = content.Replace(NullLiteral, JsonNullLiteral);
            return new DevHierarchyFactory().CreateHierarchy(content, mapName, debug);
        }
    }
}