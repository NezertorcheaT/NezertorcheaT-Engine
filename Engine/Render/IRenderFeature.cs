using Engine.Render.Symbols;

namespace Engine.Render
{
    public interface IRenderFeature
    {
        void RenderProcedure(SymbolMatrix matrix);
    }
}