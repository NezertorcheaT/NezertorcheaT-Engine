using System.Threading;
using Engine.Core;

public static class Program
{
    public static void Main(string[] args)
    {
        Startup.Start();
        if (GameConfig.SceneManager.CurrentHierarchy is null) return;

        Thread drawLoop = new Thread(Startup.DrawCycle);
        Thread mainLoop = new Thread(Startup.MainLoop);
        Thread fixedLoop = new Thread(Startup.FixedLoop);

        drawLoop.Start();
        mainLoop.Start();
        fixedLoop.Start();

        fixedLoop.Join();
        drawLoop.Join();
        mainLoop.Join();
    }
}