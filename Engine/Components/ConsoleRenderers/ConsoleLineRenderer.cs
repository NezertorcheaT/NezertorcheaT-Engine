using System;
using System.Collections.Generic;
using System.Numerics;
using Engine.Core;
using Engine.Render.Symbols;
using Engine.Scene;

namespace Engine.Components.ConsoleRenderers
{
    public class ConsoleLineRenderer : Component, IRenderer
    {
        public char Character = ' ';
        public int Color = 14;
        public List<double[]> Positions;

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);


            for (var i = 0; i < Positions.Count - 1; i++)
            {
                DrawLineSymbol(
                    matrix,
                    transform.Position + Positions[i].Da2V2(),
                    transform.Position + Positions[i + 1].Da2V2(),
                    new Symbol {Character = Character, Color = (ConsoleColor) Color},
                    cam
                );
            }
        }

        private static void DrawLineSymbol(SymbolMatrix symbolMatrix, Vector2 pos1, Vector2 pos2, Symbol symbol,
            Camera? camera)
        {
            if (!SymbolMatrix.WorldToSymbolMatrixPosition(ref pos1, camera, true)) return;
            if (!SymbolMatrix.WorldToSymbolMatrixPosition(ref pos2, camera, true)) return;

            symbolMatrix.Draw(symbol, pos1);
            symbolMatrix.Draw(symbol, pos2);

            var delay = Vector2.Distance(pos1, pos2) / GameConfig.Data.CONSOLE_LINE_RENDERER_DELAY;
            var dir = Vector2.Normalize(pos2 - pos1);

            for (var i = pos1; Vector2.Distance(i, pos2) > delay; i += dir * delay)
            {
                symbolMatrix.Draw(symbol, i);
            }
        }
    }
}