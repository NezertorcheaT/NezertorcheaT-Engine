using ConsoleEngine.Symbols;

namespace ConsoleEngine.Components
{
    public interface IRenderer : IComponentInit
    {
        void OnDraw(SymbolMatrix matrix);
    }
}