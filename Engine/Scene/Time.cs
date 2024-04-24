using Engine.Core;

namespace Engine.Scene
{
    public static class Time
    {
        public static double FixedDeltaTime => GameConfig.Data.FIXED_REPETITIONS;
        public static double DeltaTime { get; private set; }

        internal static void SetDeltaTime(double deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}