using System;
using ConsoleEngine.Scene;
using ConsoleEngine.Symbols;

namespace ConsoleEngine.Components.ConsoleRenderers
{
    public class ConsoleSymbolStringRenderer : Component, IRenderer
    {
        public string Path = "";
        public int Color = 14;

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);

            var pos = transform.Position;
            var symbolString = SymbolStringFactory.GetFromPath(Path);
            symbolString.Color = (ConsoleColor) Color;

            if (SymbolMatrix.WorldToSymbolMatrixPosition(ref pos, cam, true))
            {
                SymbolString.PlaceAt(matrix, symbolString, pos);
            }
        }
    }
}