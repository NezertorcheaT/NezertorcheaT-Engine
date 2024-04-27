using System;
using System.Numerics;
using Engine.Components;
using Engine.Render.Symbols;
using Engine.Scene;

namespace GameProject
{
    public class Fill : Component, IRenderer, IComponentStart
    {
        public char Character = ' ';
        public ConsoleColor Color = ConsoleColor.White;
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
            var pos = transform.Position;
            var scale = new Vector2(Math.Abs(Scale.X), Math.Abs(Scale.Y));
            var delay = 1f / Symbol.Aspect;


            for (var x = 0f; x < scale.X; x += delay)
            {
                for (var y = 0f; y < scale.Y; y += delay)
                {
                    var v = pos + new Vector2(x, y) - scale / 2f;
                    if (SymbolMatrix.WorldToSymbolMatrixPosition(ref v, _camera, true))
                        SymbolMatrix.Draw(new Symbol(Character, Color), v, matrix);
                }
            }
        }
    }
}