using System;
using System.Numerics;
using ConsoleEngine.Scene;
using ConsoleEngine.Symbols;

namespace ConsoleEngine.Components.ConsoleRenderers
{
    public class ConsoleRenderer : Component, IRenderer
    {
        public char Character = ' ';
        public int Color = 14;

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);

            DrawSymbol(
                matrix,
                transform.Position,
                new Symbol {Character = Character, Color = (ConsoleColor) Color},
                cam
            );
        }

        private static void DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol, Camera? camera)
        {
            if (SymbolMatrix.WorldToSymbolMatrixPosition(ref worldPos, camera))
                symbolMatrix.Draw(symbol, worldPos);
        }
    }
}