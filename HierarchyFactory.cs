using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using ConsoleEngine.Components;

namespace ConsoleEngine
{
    public static class HierarchyFactory
    {
        public static string SaveHierarchy(Hierarchy hierarchy)
        {
            var s = "";
            return s;
        }

        public static Hierarchy CreateHierarchy(string mapPath)
        {
            var path = $"maps\\{mapPath}.json";
            var jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions {WriteIndented = true};
            var node = JsonNode.Parse(jsonString)!;
            var hierarchy = new Hierarchy();

            Logger.Log(path);
            Logger.Log(options);
            Logger.Log(hierarchy);

            List<string[]> parents = new List<string[]>(node.AsArray().Count);

            foreach (var objNode in node.AsArray())
            {
                var gameObj = new GameObject(objNode["name"].ToString(), objNode["tag"].ToString(),
                    objNode["layer"].Deserialize<int>(), hierarchy);

                gameObj.transform.LocalPosition = new Vector2(
                    objNode["components"].AsArray().First()["data"].AsArray().First()["value"].AsArray().First()
                        .Deserialize<float>(),
                    objNode["components"].AsArray().First()["data"].AsArray().First()["value"].AsArray().Last()
                        .Deserialize<float>()
                );

                parents.Add(new[]
                {
                    gameObj.name,
                    objNode["components"].AsArray().First()["data"].AsArray()[1]["value"].Deserialize<string>()
                });

                Logger.Log(gameObj);
                foreach (var componentNode in objNode["components"].AsArray())
                {
                    if (componentNode["name"].ToString() == nameof(Transform) ||
                        componentNode["name"].ToString() == nameof(Behavior)) continue;

                    var comp = (Activator.CreateInstance(
                        ReflectiveEnumerator.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode["name"].ToString() == component.Name)) as Component);

                    if (comp == null) continue;
                    Logger.Log(comp);

                    foreach (var varNode in componentNode["data"].AsArray())
                    {
                        Logger.Log(varNode["value"]);
                        foreach (var fieldInfo in comp.GetType().GetFields())
                        {
                            if (fieldInfo.Name != varNode["name"].ToString()) continue;
                            Logger.Log(fieldInfo.FieldType.Name);
                            Logger.Log(fieldInfo.Name);
                            fieldInfo.SetValue(comp, varNode["value"].Deserialize(fieldInfo.FieldType));
                            break;
                        }
                    }

                    gameObj.AddComponent(comp);
                }

                hierarchy.Objs.Add(gameObj);
            }

            foreach (var par in parents)
            {
                var parent = par[1];
                if (parent == "null") continue;

                var child = par[0];
                var childObj = GameObject.FindObjectByName(child, hierarchy);
                var parentObj = GameObject.FindObjectByName(parent, hierarchy);

                if (childObj != null) childObj.transform.Parent = parentObj?.transform;
            }

            return hierarchy;
        }
    }
}