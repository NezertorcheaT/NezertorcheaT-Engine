using System.Numerics;
using Engine.Components;

namespace Engine
{
    public interface IRenderer : IComponentInit
    {
        void DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol);

        void DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol, Camera? camera, GameConfigData gcd,
            Vector2 cameraPosition);
    }
}