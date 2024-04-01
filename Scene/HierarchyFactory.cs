using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using ConsoleEngine.Components;
using ConsoleEngine.IO;

namespace ConsoleEngine.Scene
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
        private static readonly string ComponentDataNameLiteral = "name";
        private static readonly string ComponentEnabledLiteral = "enabled";
        private static readonly string NullLiteral = "null";

        public static string SaveHierarchy(Hierarchy hierarchy)
        {
            var s = "";
            return s;
        }

        public static Hierarchy CreateHierarchy(string path)
        {
            var jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions {WriteIndented = true};
            var node = JsonNode.Parse(jsonString)!;
            var hierarchy = new Hierarchy();

            StaticContainersFactory.CreateStaticContainers(hierarchy);

            Logger.Log(path, "map path");
            Logger.Log(options, "map json options");

            List<string[]> parents = new List<string[]>(node.AsArray().Count);

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

                gameObj.transform.LocalPosition = new Vector2(
                    trcmp.First()[ComponentDataValueLiteral].AsArray().First().Deserialize<float>(),
                    trcmp.First()[ComponentDataValueLiteral].AsArray().Last().Deserialize<float>()
                );

                var localRotation = trcmp[1][ComponentDataValueLiteral].Deserialize<float>();
                gameObj.transform.LocalRotation = localRotation;
                Logger.Log(localRotation, $"{gameObj.name}'s localRotation");

                parents.Add(new[]
                {
                    gameObj.name,
                    trcmp.Last()[ComponentDataValueLiteral].Deserialize<string>()
                });

                Logger.Log(gameObj, "GameObject to initialize");
                foreach (var componentNode in objNode[ComponentsLiteral].AsArray())
                {
                    if (componentNode[ComponentDataNameLiteral].ToString() == nameof(Transform) ||
                        componentNode[ComponentDataNameLiteral].ToString() == nameof(Behavior) ||
                        componentNode[ComponentDataNameLiteral].ToString() == nameof(Component)) continue;

                    var comp = Activator.CreateInstance(
                        Helper.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode[ComponentDataNameLiteral].ToString() == component.Name)) as Component;

                    if (comp == null) continue;
                    comp.enabled = componentNode[ComponentEnabledLiteral].Deserialize<bool>();
                    
                    Logger.Log(comp, "component to initialize");

                    foreach (var varNode in componentNode[ComponentDataLiteral].AsArray())
                    {
                        Logger.Log(varNode[ComponentDataValueLiteral], "component field to initialize");
                        foreach (var fieldInfo in comp.GetType()
                            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (fieldInfo.Name != varNode[ComponentDataNameLiteral].ToString()) continue;
                            Logger.Log(fieldInfo.FieldType.Name);
                            Logger.Log(fieldInfo.Name);
                            fieldInfo.SetValue(comp,
                                varNode[ComponentDataValueLiteral].Deserialize(fieldInfo.FieldType));
                            break;
                        }
                    }

                    gameObj.AddComponent(comp);
                }

                hierarchy.Objects.Add(gameObj);
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