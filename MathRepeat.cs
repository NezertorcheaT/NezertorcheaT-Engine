namespace Engine
{
    public static class MathRepeat
    {
        public static double Repeat(double inp, double max)
        {
            if (inp <= max)
            {
                return inp < 0 ? Repeat(max - inp, max) : inp;
            }

            return inp > max ? Repeat(inp - max, max) : inp;
        }
    }
}