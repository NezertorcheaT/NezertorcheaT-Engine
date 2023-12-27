using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConsoleEngine.IO
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
        public static readonly string DefaultConfig =
            "{\"HEIGHT\": 15,\"WIDTH\": 36,\"MAP\": \"maps\\main.json\",\"COLLISION_SUBSTEPS\": 1,\"RIGIDBODY_SUBSTEPS\": 1,\"FPS\": 256,\"FIXED_REPETITIONS\": 0.02,\"LOG_DRAWCALLS\": false,\"DRAW_BUFFER_SIZE\":1,\"CONSOLE_LINE_RENDERER_DELAY\": 8.0, \"RESIZE_WINDOW\": true, \"START_RESIZE_WINDOW\": true}";

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
}