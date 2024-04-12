using System.Threading;
using ConsoleEngine.IO;

public static class Program
{
    public static void Main(string[] args)
    {    
        Startup.Start();
        if (GameConfig.GameHierarchy == null) return;
            
        Thread draw = new Thread(Startup.DrawCycle);
        Thread main = new Thread(Startup.MainLoop);
            
        draw.Start();
        main.Start();
    }
}