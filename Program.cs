namespace ConsoleEngine
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Startup.Start(out var hierarchy);
            
            if (hierarchy != null)
            {
                Startup.DrawCycle(hierarchy);
                Startup.MainLoop(hierarchy);
            }

            //Startup.Stop();
        }
    }
}