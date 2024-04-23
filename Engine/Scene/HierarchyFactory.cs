using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.Components;
using Engine.Core;
using Engine.Scene.Serializing;

namespace Engine.Scene
{
    public static class HierarchyFactory
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

        public static string SaveHierarchy(Hierarchy hierarchy)
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
                    componentJson.Add(ComponentDataNameLiteral, component.GetType().AssemblyQualifiedName);
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
                            Logger.Log($"PremadeSerializationFunctions contains {field.FieldType.AssemblyQualifiedName}");
                            fieldData.Add(ComponentDataValueLiteral,
                                SerializingHelper.PremadeSerializationFunctions[field.FieldType.Name](
                                    field.GetValue(component)));
                        }
                        else
                        {
                            Logger.Log($"PremadeSerializationFunctions not contains {field.FieldType.AssemblyQualifiedName}");
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

        public static Hierarchy CreateHierarchy(string path, bool debug = true)
        {
            var jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions {WriteIndented = true};
            var node = JsonNode.Parse(jsonString, new JsonNodeOptions {PropertyNameCaseInsensitive = false})!;
            var hierarchy = new Hierarchy();
            (hierarchy as ISetHierarchyName).MapName = Path.GetFileName(path);

            if (debug) Logger.Log(path, "map path");
            if (debug) Logger.Log(hierarchy.MapName, "map name");
            if (debug) Logger.Log(options, "map json options");

            List<string[]> parents = new List<string[]>(node.AsArray().Count);
            List<Tuple<string, int, string, string>> gObjects = new List<Tuple<string, int, string, string>>(1);

            foreach (var objNode in node.AsArray())
            {
                var gameObj = new GameObject(
                    objNode[GameObjectNameLiteral].ToString(),
                    objNode[GameObjectTagLiteral].ToString(),
                    objNode[GameObjectLayerLiteral].Deserialize<int>(),
                    hierarchy
                );
                gameObj.active = objNode[GameObjectActiveLiteral].Deserialize<bool>();

                var trcmp = objNode[ComponentsLiteral].AsArray().First()[ComponentDataLiteral].AsArray();

                gameObj.transform.LocalPosition =
                    (Vector2) SerializingHelper.PremadeDeserializationFunctions["Vector2"](
                        trcmp.First()[ComponentDataValueLiteral]);

                var localRotation = trcmp[1][ComponentDataValueLiteral].Deserialize<float>();
                gameObj.transform.LocalRotation = localRotation;

                if (debug) Logger.Log(localRotation, $"{gameObj.name}'s localRotation");

                parents.Add(new[]
                {
                    gameObj.name,
                    trcmp.Last()[ComponentDataValueLiteral].Deserialize<string>()
                });

                var compInd = 1;
                if (debug) Logger.Log(gameObj, "GameObject to initialize");
                foreach (var componentNode in objNode[ComponentsLiteral].AsArray())
                {
                    if (componentNode[ComponentNameLiteral].ToString() == nameof(Transform) ||
                        componentNode[ComponentNameLiteral].ToString() == nameof(Behavior) ||
                        componentNode[ComponentNameLiteral].ToString() == nameof(Component)) continue;

                    if (debug) Logger.Log(componentNode[ComponentNameLiteral].ToString(), "component name");

                    var comp = Activator.CreateInstance(
                        Helper.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode[ComponentNameLiteral].ToString() == component.Name)) as Component;

                    if (comp == null) continue;
                    comp.enabled = componentNode[ComponentEnabledLiteral].Deserialize<bool>();

                    if (debug) Logger.Log(comp, "component to initialize");

                    foreach (var varNode in componentNode[ComponentDataLiteral].AsArray())
                    {
                        if (debug) Logger.Log(varNode[ComponentDataValueLiteral], "component field to initialize");
                        foreach (var fieldInfo in comp.GetType()
                            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (fieldInfo.Name != varNode[ComponentDataNameLiteral].ToString()) continue;
                            if (debug) Logger.Log(fieldInfo.FieldType.Name);
                            if (debug) Logger.Log(fieldInfo.Name);
                            if (SerializingHelper.PremadeDeserializationFunctions.ContainsKey(fieldInfo.FieldType.Name))
                            {
                                if (debug)
                                    Logger.Log($"PremadeDeserializationFunctions contains {fieldInfo.FieldType.Name}");
                                fieldInfo.SetValue(comp,
                                    SerializingHelper.PremadeDeserializationFunctions[fieldInfo.FieldType.Name](
                                        varNode[ComponentDataValueLiteral]));
                            }
                            else if (fieldInfo.FieldType.Name == typeof(GameObject).Name)
                            {
                                gObjects.Add(new Tuple<string, int, string, string>(
                                        gameObj.name,
                                        compInd,
                                        varNode[ComponentDataNameLiteral].ToString(),
                                        varNode[ComponentDataValueLiteral].Deserialize<string>()
                                    )
                                );
                            }
                            else
                            {
                                if (debug)
                                    Logger.Log(
                                        $"PremadeDeserializationFunctions not contains {fieldInfo.FieldType.Name}");
                                fieldInfo.SetValue(comp,
                                    varNode[ComponentDataValueLiteral].Deserialize(fieldInfo.FieldType));
                            }

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
                        "gameobject fields initializer");
                if (gObject is null) continue;
                var orgObj = GameObject.FindObjectByName(gObject.Item1, hierarchy);
                if (debug) Logger.Log(orgObj.name, "gameobject fields initializer");
                Component comObj = null;
                var coml = orgObj.GetAllComponents<Component>().ToArray();
                for (var i = 0; i < coml.Length; i++)
                {
                    if (i != gObject.Item2) continue;
                    comObj = coml[i];
                    break;
                }

                if (comObj is null) continue;
                if (debug) Logger.Log(comObj.GetType().Name, "gameobject fields initializer");

                foreach (var fieldInfo in comObj.GetType().GetFields())
                {
                    if (fieldInfo.Name == gObject.Item3)
                    {
                        if (debug) Logger.Log(fieldInfo.Name, "gameobject fields initializer");
                        var gg = GameObject.FindObjectByName(gObject.Item4, hierarchy);
                        if (debug) Logger.Log(gg?.name, "gameobject fields initializer");
                        fieldInfo.SetValue(comObj, gg);
                        break;
                    }
                }
            }

            foreach (var par in parents)
            {
                var parent = par[1];
                if (parent == NullLiteral) continue;

                var child = par[0];
                var childObj = GameObject.FindObjectByName(child, hierarchy);
                var parentObj = GameObject.FindObjectByName(parent, hierarchy);

                if (childObj != null) childObj.transform.Parent = parentObj?.transform;
            }

            return hierarchy;
        }
    }
}