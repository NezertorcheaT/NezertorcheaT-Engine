using ConsoleEngine.Components;

namespace ConsoleEngine
{
    public interface IRenderer : IComponentInit
    {
        void OnDraw(SymbolMatrix matrix);
    }
}