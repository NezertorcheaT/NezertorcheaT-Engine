using Engine.Render.Symbols;

namespace Engine.Components
{
    public interface IRenderer : IComponentInit
    {
        void OnDraw(SymbolMatrix matrix);
    }
}