using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.Components;
using Engine.Core;

namespace Engine.Scene.Serializing
{
    public static class SerializingHelper
    {
        public static Dictionary<string, Func<object?, JsonNode>> PremadeSerializationFunctions =
            new Dictionary<string, Func<object?, JsonNode>>
            {
                {
                    "Vector2", vec =>
                    {
                        var v = (Vector2?) vec;

                        Logger.Log($"Object {vec}", tabs: 1);
                        if (v.HasValue) return new JsonArray {v.Value.X, v.Value.Y};

                        Logger.Log($"Object {vec} is not a {nameof(Vector2)}", "argument error", tabs: 1);
                        return new JsonArray {0, 0};
                    }
                },
                {
                    "Vector3", vec =>
                    {
                        var v = (Vector3?) vec;

                        if (v.HasValue) return new JsonArray {v.Value.X, v.Value.Y, v.Value.Z};

                        Logger.Log($"Object {vec} is not a {nameof(Vector3)}", "argument error", tabs: 1);
                        return new JsonArray {0, 0, 0};
                    }
                },
                {
                    "Vector2Int", vec =>
                    {
                        var v = (Vector2?) vec;

                        if (v.HasValue) return new JsonArray {v.Value.X, v.Value.Y};

                        Logger.Log($"Object {vec} is not a {nameof(Vector2)}", "argument error", tabs: 1);
                        return new JsonArray {0, 0};
                    }
                },
                {
                    "Vector3Int", vec =>
                    {
                        var v = (Vector3?) vec;

                        if (v.HasValue) return new JsonArray {v.Value.X, v.Value.Y, v.Value.Z};

                        Logger.Log($"Object {vec} is not a {nameof(Vector3)}", "argument error", tabs: 1);
                        return new JsonArray {0, 0, 0};
                    }
                },
                {
                    "Bounds", bounds =>
                    {
                        var b = (Bounds) bounds;
                        return new JsonObject
                        {
                            {"Size", PremadeSerializationFunctions["Vector2"](b.Size)},
                            {"Position", PremadeSerializationFunctions["Vector2"](b.Position)}
                        };
                    }
                },
                {
                    "GameObject", obj =>
                    {
                        var gameObject = (GameObject) obj;
                        var ar = new JsonArray {obj is null ? "null" : gameObject.name, -1};
                        return ar;
                    }
                },
                {
                    "Component", obj =>
                    {
                        var comp = (Component) obj;
                        var ar = new JsonArray {obj is null ? "null" : comp.gameObject.name, 0};
                        return ar;
                    }
                },
            };

        public static Dictionary<string, Func<JsonNode, object>> PremadeDeserializationFunctions =
            new Dictionary<string, Func<JsonNode, object>>
            {
                {
                    "Vector2",
                    node => new Vector2(node.AsArray()[0].Deserialize<float>(), node.AsArray()[1].Deserialize<float>())
                },
                {
                    "Vector3",
                    node => new Vector3(node.AsArray()[0].Deserialize<float>(), node.AsArray()[1].Deserialize<float>(),
                        node.AsArray()[2].Deserialize<float>())
                },
                {
                    "Vector2Int",
                    node => new Vector2(node.AsArray()[0].Deserialize<int>(), node.AsArray()[1].Deserialize<int>())
                },
                {
                    "Vector3Int",
                    node => new Vector3(node.AsArray()[0].Deserialize<int>(), node.AsArray()[1].Deserialize<int>(),
                        node.AsArray()[2].Deserialize<int>())
                },
                {
                    "Bounds",
                    node => new Bounds(
                        (Vector2) PremadeDeserializationFunctions["Vector2"](node["Size"]),
                        (Vector2) PremadeDeserializationFunctions["Vector2"](node["Position"])
                    )
                },
            };
    }
}