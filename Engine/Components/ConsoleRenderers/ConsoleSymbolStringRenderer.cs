using System;
using Engine.Render.Symbols;
using Engine.Scene;

namespace Engine.Components.ConsoleRenderers
{
    public class ConsoleSymbolStringRenderer : Component, IRenderer
    {
        public string Path = "";
        public ConsoleColor Color = ConsoleColor.White;

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);

            var pos = transform.Position;
            var symbolString = SymbolStringFactory.GetFromPath(Path);
            symbolString.Color = Color;

            if (SymbolMatrix.WorldToSymbolMatrixPosition(ref pos, cam, true))
                SymbolString.PlaceAt(matrix, symbolString, pos);
        }
    }
}