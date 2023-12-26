namespace ConsoleEngine
{
    public static class Time
    {
        public static double FixedDeltaTime => GameConfig.Data.FIXED_REPETITIONS;
        public static double DeltaTime => GameConfig.Data.FIXED_REPETITIONS;
    }
}