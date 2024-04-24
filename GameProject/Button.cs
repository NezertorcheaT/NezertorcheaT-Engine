using System;
using System.Numerics;
using Engine.Components;
using Engine.Core;
using Engine.Render.Symbols;
using Engine.Scene;

namespace GameProject
{
    public class Button : Component, IRenderer, IComponentStart, IComponentUpdate
    {
        public char CharacterFill = ' ';
        public char CharacterBorders = '#';
        public string Text = "Button";
        public int Color = 14;
        public Vector2 Scale = new Vector2(5, 5);
        private Camera? _camera;

        void IComponentStart.Start()
        {
            _camera = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);
        }

        void IComponentUpdate.Update()
        {
            var v = Input.ConsoleToWorldPosition(Input.ScreenToConsolePosition(Input.GetCursorPosition()), _camera);
            var b = new Bounds(transform.Position + Scale / 2f, Scale);
            if (b.Contains(v) && Input.GetKey(Input.Keys.E))
                Logger.Log("fire in ze hole");
        }

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            if (_camera is null) return;
            var pos = transform.Position;
            var scale = new Vector2(Math.Abs(Scale.X), Math.Abs(Scale.Y));
            var delay = 1f/Symbol.Aspect;


            for (var x = 0f; x < scale.X; x += delay)
            {
                for (var y = 0f; y < scale.Y; y += delay)
                {
                    matrix.Draw(new Symbol(CharacterFill, (ConsoleColor) Color),
                        SymbolMatrix.WorldToSymbolMatrixPosition(pos + new Vector2(x, y), _camera, true));
                }
            }

            for (var x = pos.X / delay; x < (pos.X + scale.X) / delay; x++)
            {
                for (var y = pos.Y / delay; y < (pos.Y + scale.Y) / delay; y++)
                {
                    if (
                        Math.Abs(x - pos.X / delay) < delay / 2f ||
                        Math.Abs(x - (pos.X + scale.X) / delay) < delay  ||
                        Math.Abs(y - pos.Y / delay) < delay / 2f ||
                        Math.Abs(y - (pos.Y + scale.Y) / delay) < delay / 2f
                    )
                    {
                        matrix.Draw(new Symbol(CharacterBorders, (ConsoleColor) Color),
                            SymbolMatrix.WorldToSymbolMatrixPosition(new Vector2(x, y) * delay, _camera, true));
                    }
                }
            }

            var ss = new SymbolString(Text);
            SymbolString.PlaceAt(matrix, ss, SymbolMatrix.WorldToSymbolMatrixPosition(
                pos + scale / 2f, _camera, true) + ss.Bounds.Extends.Multiply(new Vector2(-1, 0.25f)));
        }
    }
}