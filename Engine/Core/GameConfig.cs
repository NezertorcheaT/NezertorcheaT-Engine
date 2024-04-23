using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.Render;
using Engine.Scene;

namespace Engine.Core
{
    /// <summary>
    /// Game settings
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// Current game settings
        /// </summary>
        public static GameConfigData Data { get; private set; }

        public static readonly string StaticContainersPath = "staticContainers.txt";

        public static SceneManager SceneManager;
        public static IRenderFeature RenderFeature { get; private set; }

        public static void SetupHierarchy(Func<IEnumerable<Hierarchy>> factory)
        {
            SceneManager = new SceneManager();
            (SceneManager as IGameConfigSceneManager).InitializeHierarchies(factory.Invoke().ToArray());
        }

        public static void SetupRenderFeature()
        {
            var compType = Helper.GetEnumerableOfType<IRenderFeature>()
                .FirstOrDefault(component => Data.RENDER_FEATURE == component.Name, null);

            if (compType == null)
            {
                RenderFeature = new BaseFastRenderFeature();
                return;
            }

            RenderFeature = Activator.CreateInstance(compType) as IRenderFeature ?? new BaseFastRenderFeature();
        }

        /// <summary>
        /// Rewrite current config file
        /// </summary>
        public static void SaveCurrentConfig()
        {
            var stream = File.CreateText("config.json");
            stream.Write((Data as IJsonable).Json);
            stream.Close();
        }

        /// <summary>
        /// Default config
        /// </summary>
        public static readonly string DefaultConfig = @"
{
    ""HEIGHT"": 15,
    ""WIDTH"": 36,
    ""MAPS"": [""maps\\main.json""],
    ""RENDER_FEATURE"": ""BaseRenderFeature"",
    ""COLLISION_SUBSTEPS"": 1,
    ""FPS"": 256,
    ""FIXED_REPETITIONS"": 0.02,
    ""LOG_DRAWCALLS"": false,
    ""DRAW_BUFFER_SIZE"":1,
    ""CONSOLE_LINE_RENDERER_DELAY"": 8.0,
    ""RESIZE_WINDOW"": true,
    ""START_RESIZE_WINDOW"": true,
    ""DRAW_PRIOIRITY"": false,
    ""USE_AABB"": true
}
";

        /// <summary>
        /// Updates current game settings
        /// </summary>
        /// <returns></returns>
        public static GameConfigData GetData()
        {
            var jsonString = File.ReadAllText("config.json");
            var options = new JsonSerializerOptions {WriteIndented = true};
            JsonNode node = JsonNode.Parse(jsonString) ?? JsonNode.Parse(DefaultConfig)!;
            Data = node.Deserialize<GameConfigData>()!;
            return Data;
        }
    }

    public class SceneManager : IGameConfigSceneManager
    {
        private Hierarchy[] _hierarchies = new Hierarchy[1];
        public Hierarchy CurrentHierarchy => _hierarchies[CurrentHierarchyNumber];
        IEnumerable<Hierarchy> IGameConfigSceneManager.Hierarchies => _hierarchies;
        public int CurrentHierarchyNumber { get; private set; }

        public void SetScene(int sceneNumber)
        {
            CurrentHierarchyNumber = sceneNumber;
        }

        void IGameConfigSceneManager.InitializeHierarchies(Hierarchy[] hierarchies)
        {
            _hierarchies = hierarchies;
        }
    }

    internal interface IGameConfigSceneManager
    {
        void InitializeHierarchies(Hierarchy[] hierarchies);
        IEnumerable<Hierarchy> Hierarchies { get; }
    }
}