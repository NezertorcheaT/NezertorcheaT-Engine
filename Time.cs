using ConsoleEngine.IO;

namespace ConsoleEngine
{
    public static class Time
    {
        public static double FixedDeltaTime => GameConfig.Data.FIXED_REPETITIONS;
        public static double DeltaTime { get; private set; }

        /// <summary>
        /// pls, DO NOT USE!!!!!!
        /// </summary>
        /// <param name="dt"></param>
        public static void SetDeltaTime(double dt)
        {
            DeltaTime = dt;
        }
    }
}