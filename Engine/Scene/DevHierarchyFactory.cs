using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.Components;
using Engine.Core;
using Engine.Scene.Serializing;

namespace Engine.Scene
{
    public class DevHierarchyFactory : IHierarchyFactory
    {
        private static readonly string GameObjectNameLiteral = "name";
        private static readonly string GameObjectTagLiteral = "tag";
        private static readonly string GameObjectLayerLiteral = "layer";
        private static readonly string GameObjectActiveLiteral = "active";
        private static readonly string ComponentsLiteral = "components";
        private static readonly string ComponentDataLiteral = "data";
        private static readonly string ComponentDataValueLiteral = "value";
        private static readonly string ComponentNameLiteral = "name";
        private static readonly string ComponentDataNameLiteral = "name";
        private static readonly string ComponentEnabledLiteral = "enabled";
        private static readonly string NullLiteral = "null";

        public string SaveHierarchy(Hierarchy hierarchy)
        {
            var node = new JsonArray();
            foreach (var gameObject in hierarchy.Objects)
            {
                var objectJson = new JsonObject();
                objectJson.Add(GameObjectNameLiteral, gameObject.name);
                objectJson.Add(GameObjectTagLiteral, gameObject.tag);
                objectJson.Add(GameObjectLayerLiteral, gameObject.layer);
                objectJson.Add(GameObjectActiveLiteral, gameObject.active);

                var componentsJson = new JsonArray();
                foreach (var component in gameObject.GetAllComponents<Component>())
                {
                    var componentJson = new JsonObject();
                    componentJson.Add(ComponentDataNameLiteral, component.GetType().Name);
                    componentJson.Add(ComponentEnabledLiteral, component.enabled);

                    var dataJson = new JsonArray();
                    var componentType = component.GetType();
                    var componentFields = componentType.GetFields();
                    foreach (var field in componentFields)
                    {
                        var fieldData = new JsonObject();
                        fieldData.Add(ComponentDataNameLiteral, field.Name);

                        if (SerializingHelper.PremadeSerializationFunctions.ContainsKey(field.FieldType.Name))
                        {
                            Logger.Log(
                                $"PremadeSerializationFunctions contains {field.FieldType.Name}");
                            fieldData.Add(ComponentDataValueLiteral,
                                SerializingHelper.PremadeSerializationFunctions[field.FieldType.Name](
                                    field.GetValue(component)));
                        }
                        else if (field.FieldType.IsSubclassOf(typeof(Component)))
                        {
                            Logger.Log(
                                $"PremadeSerializationFunctions contains ({nameof(Component)}){field.FieldType.Name}");
                            fieldData.Add(ComponentDataValueLiteral,
                                SerializingHelper.PremadeSerializationFunctions[nameof(Component)](
                                    field.GetValue(component)));
                        }
                        else
                        {
                            Logger.Log(
                                $"PremadeSerializationFunctions not contains {field.FieldType.Name}");
                            fieldData.Add(ComponentDataValueLiteral,
                                JsonSerializer.SerializeToNode(field.GetValue(component), field.FieldType));
                        }

                        dataJson.Add(fieldData);
                    }

                    componentJson.Add(ComponentDataLiteral, dataJson);

                    componentsJson.Add(componentJson);
                }

                objectJson.Add(ComponentsLiteral, componentsJson);

                node.Add(objectJson);
            }

            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            return node.ToJsonString(options);
        }

        public Hierarchy CreateHierarchy(string path, bool debug = true)
        {
            var jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions {WriteIndented = true};
            var node = JsonNode.Parse(jsonString, new JsonNodeOptions {PropertyNameCaseInsensitive = false})!;
            var hierarchy = new Hierarchy();
            (hierarchy as ISetHierarchyName).MapName = Path.GetFileName(path);

            if (debug) Logger.Log(path, "map path");
            if (debug) Logger.Log(hierarchy.MapName, "map name");
            if (debug) Logger.Log(options, "map json options");

            List<Tuple<string, int, string, Tuple<string, int>>> gObjects =
                new List<Tuple<string, int, string, Tuple<string, int>>>(1);

            foreach (var objNode in node.AsArray())
            {
                var gameObj = new GameObject(
                    objNode[GameObjectNameLiteral].ToString(),
                    objNode[GameObjectTagLiteral].ToString(),
                    objNode[GameObjectLayerLiteral].Deserialize<int>(),
                    hierarchy
                );
                gameObj.active = objNode[GameObjectActiveLiteral].Deserialize<bool>();

                var compInd = 0;
                if (debug) Logger.Log(gameObj, "GameObject to initialize", 1);
                foreach (var componentNode in objNode[ComponentsLiteral].AsArray())
                {
                    if (componentNode[ComponentNameLiteral].ToString() == nameof(Behavior) ||
                        componentNode[ComponentNameLiteral].ToString() == nameof(Component)) continue;

                    if (debug) Logger.Log(componentNode[ComponentNameLiteral].ToString(), "component name", 2, 1, "");

                    var comp = Activator.CreateInstance(
                        Helper.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode[ComponentNameLiteral].ToString() == component.Name)) as Component;

                    if (comp == null) continue;
                    comp.enabled = componentNode[ComponentEnabledLiteral].Deserialize<bool>();

                    if (debug) Logger.Log(comp, "component to initialize", 2, 1, "");

                    foreach (var varNode in componentNode[ComponentDataLiteral].AsArray())
                    {
                        if (debug)
                            Logger.Log(
                                varNode[ComponentDataValueLiteral].ToJsonString(new JsonSerializerOptions
                                    {WriteIndented = false}), "component field to initialize", 3, 2, "");
                        foreach (var fieldInfo in comp.GetType()
                            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (fieldInfo.Name != varNode[ComponentDataNameLiteral].ToString()) continue;
                            if (debug) Logger.Log(fieldInfo.FieldType.Name, tabs: 4, firstTabs: 3);
                            if (debug) Logger.Log(fieldInfo.Name, tabs: 4, firstTabs: 3);
                            if (SerializingHelper.PremadeDeserializationFunctions.ContainsKey(fieldInfo.FieldType.Name))
                            {
                                if (debug)
                                    Logger.Log($"PremadeDeserializationFunctions contains {fieldInfo.FieldType.Name}",
                                        tabs: 4, firstTabs: 3);
                                fieldInfo.SetValue(comp,
                                    SerializingHelper.PremadeDeserializationFunctions[fieldInfo.FieldType.Name](
                                        varNode[ComponentDataValueLiteral]));
                            }
                            else if (fieldInfo.FieldType.Name == typeof(GameObject).Name)
                            {
                                gObjects.Add(new Tuple<string, int, string, Tuple<string, int>>(
                                        gameObj.name,
                                        compInd,
                                        varNode[ComponentDataNameLiteral].ToString(),
                                        new Tuple<string, int>(
                                            varNode[ComponentDataValueLiteral].AsArray().First().Deserialize<string>(),
                                            -1
                                        )
                                    )
                                );
                            }
                            else if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
                            {
                                gObjects.Add(new Tuple<string, int, string, Tuple<string, int>>(
                                        gameObj.name,
                                        compInd,
                                        varNode[ComponentDataNameLiteral].ToString(),
                                        new Tuple<string, int>(
                                            varNode[ComponentDataValueLiteral].AsArray().First().Deserialize<string>(),
                                            varNode[ComponentDataValueLiteral].AsArray().Last().Deserialize<int>()
                                        )
                                    )
                                );
                            }
                            else
                            {
                                if (debug)
                                    Logger.Log(
                                        $"PremadeDeserializationFunctions not contains {fieldInfo.FieldType.Name}",
                                        tabs: 4, firstTabs: 3);
                                fieldInfo.SetValue(comp,
                                    varNode[ComponentDataValueLiteral].Deserialize(fieldInfo.FieldType));
                            }

                            if (debug) Logger.Log(fieldInfo.GetValue(comp), "fieldInfo.GetValue(comp)", 4, 3, "");

                            break;
                        }
                    }

                    compInd++;
                    gameObj.AddComponent(comp);
                }

                hierarchy.Objects.Add(gameObj);
            }

            foreach (var gObject in gObjects)
            {
                if (debug)
                    Logger.Log($"{gObject.Item1}, {gObject.Item2}, {gObject.Item3}, {gObject.Item4}",
                        "gameobject fields initializer", tabs: 1);
                if (gObject is null) continue;
                if (gObject.Item4.Item1 == NullLiteral) continue;
                var orgObj = GameObject.FindObjectByName(gObject.Item1, hierarchy);
                if (debug) Logger.Log(orgObj.name, "gameobject fields initializer", tabs: 2, firstTabs: 1);
                Component comObj = orgObj.GetComponentAt(gObject.Item2);

                if (comObj is null) continue;
                if (debug) Logger.Log(comObj.GetType().Name, "gameobject fields initializer", tabs: 2, firstTabs: 1);

                foreach (var fieldInfo in comObj.GetType().GetFields())
                {
                    if (fieldInfo.Name == gObject.Item3)
                    {
                        if (debug) Logger.Log(fieldInfo.Name, "gameobject fields initializer", tabs: 3, firstTabs: 2);
                        var gg = GameObject.FindObjectByName(gObject.Item4.Item1, hierarchy);
                        if (debug) Logger.Log(gg?.name, "gameobject fields initializer", tabs: 3, firstTabs: 2);
                        if (debug)
                            Logger.Log(gObject.Item4.Item2, "gameobject fields initializer", tabs: 3, firstTabs: 2);
                        if (gObject.Item4.Item2 < 0)
                            fieldInfo.SetValue(comObj, gg);
                        else
                            fieldInfo.SetValue(comObj, gg?.GetComponentAt(gObject.Item4.Item2));
                        break;
                    }
                }
            }

            return hierarchy;
        }
    }
}