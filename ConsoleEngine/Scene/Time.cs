using ConsoleEngine.IO;

namespace ConsoleEngine.Scene
{
    public static class Time
    {
        public static double FixedDeltaTime => GameConfig.Data.FIXED_REPETITIONS;
        public static double DeltaTime { get; private set; }

        /// <summary>
        /// pls, DO NOT USE!!!!!!
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void SetDeltaTime(double deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}