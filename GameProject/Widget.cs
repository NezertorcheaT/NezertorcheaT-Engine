using System;
using System.Numerics;
using Engine.Components;
using Engine.Core;
using Engine.Render.Symbols;
using Engine.Scene;

namespace GameProject
{
    public class Fill : Component, IRenderer, IComponentStart
    {
        public char Character = ' ';
        public int Color = 14;
        public Vector2 Scale;
        private Camera? _camera;

        void IComponentStart.Start()
        {
            _camera = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);
        }

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            if (_camera is null) return;
            Draw(matrix, transform.Position, Scale, new Symbol(Character, (ConsoleColor) Color), _camera);
        }

        private static void Draw(SymbolMatrix symbolMatrix, Vector2 pos, Vector2 scale, Symbol symbol,
            Camera? camera)
        {
            scale = new Vector2(Math.Abs(scale.X), Math.Abs(scale.Y));
            var delay = Vector2.Distance(pos, pos + scale) / GameConfig.Data.CONSOLE_LINE_RENDERER_DELAY;

            for (var x = pos.X; x < pos.X + scale.X; x += delay)
            {
                for (var y = pos.Y; y < pos.Y + scale.Y; y += delay)
                {
                    SymbolMatrix.Draw(symbol, SymbolMatrix.WorldToSymbolMatrixPosition(new Vector2(x, y)*delay, camera),symbolMatrix);
                }
            }
        }
    }
}