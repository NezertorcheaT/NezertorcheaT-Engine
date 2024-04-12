namespace ConsoleEngine.IO.Random
{
    public static class Random
    {
        public static int Range(int min, int max)
        {
            var rand = new System.Random();
            return rand.Next(min, max - 1);
        }

        public static double Range(double min, double max)
        {
            var rand = new System.Random();
            return rand.NextDouble() * (max - min) - min;
        }
    }
}