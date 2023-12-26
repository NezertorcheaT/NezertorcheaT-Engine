using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConsoleEngine
{
    /// <summary>
    /// Game settings
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// Current game settings
        /// </summary>
        public static GameConfigData Data => data;

        private static GameConfigData data;

        public static void SaveCurrentConfig()
        {
            var stream = File.CreateText("config.json");
            stream.Write((data as IJsonable).Json);
            stream.Close();
        }

        /// <summary>
        /// Default config
        /// </summary>
        public static readonly string DefaultConfig =
            "{\"HEIGHT\": 15,\"WIDTH\": 36,\"MAP\": \"main\",\"COLLISION_SUBSTEPS\": 1,\"RIGIDBODY_SUBSTEPS\": 1,\"FPS\": 256,\"FIXED_REPETITIONS\": 0.02,\"LOG_DRAWCALLS\": false,\"DRAW_BUFFER_SIZE\":1,\"CONSOLE_LINE_RENDERER_DELAY\": 8.0}";

        /// <summary>
        /// Updates current game settings
        /// </summary>
        /// <returns></returns>
        public static GameConfigData GetData()
        {
            var jsonString = File.ReadAllText("config.json");
            var options = new JsonSerializerOptions {WriteIndented = true};
            JsonNode node = JsonNode.Parse(jsonString) ?? JsonNode.Parse(DefaultConfig)!;
            data = JsonSerializer.Deserialize<GameConfigData>(node)!;
            return data;
        }
    }
}