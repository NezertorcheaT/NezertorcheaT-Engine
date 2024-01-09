using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using ConsoleEngine.Components;
using ConsoleEngine.IO;

namespace ConsoleEngine.Scene
{
    public static class HierarchyFactory
    {
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

            Logger.Log(path,"map path");
            Logger.Log(options, "map json options");

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

                Logger.Log(gameObj,"GameObject to initialize");
                foreach (var componentNode in objNode["components"].AsArray())
                {
                    if (componentNode["name"].ToString() == nameof(Transform) ||
                        componentNode["name"].ToString() == nameof(Behavior) ||
                        componentNode["name"].ToString() == nameof(Component)) continue;

                    var comp = (Activator.CreateInstance(
                        StaticShit.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode["name"].ToString() == component.Name)) as Component);

                    if (comp == null) continue;
                    Logger.Log(comp, "component to initialize");

                    foreach (var varNode in componentNode["data"].AsArray())
                    {
                        Logger.Log(varNode["value"], "component field to initialize");
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

                hierarchy.Objects.Add(gameObj);
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