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
        public ConsoleColor Color = ConsoleColor.White;
        public Vector2 Scale = new Vector2(5, 5);
        public event Action OnClick;
        private Camera? _camera;

        void IComponentStart.Start()
        {
            _camera = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);
        }

        void IComponentUpdate.Update()
        {
            if (!ActiveAndEnabled) return;
            var v = Input.ScreenToWorldPosition(Input.GetCursorPosition(), _camera);
            var b = new Bounds(Scale, transform.Position + Scale / 2f);
            if (Input.GetKey(Input.Keys.E) && b.Contains(v))
                OnClick?.Invoke();
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
                    var v = pos + new Vector2(x, y);
                    if (SymbolMatrix.WorldToSymbolMatrixPosition(ref v, _camera, true))
                        SymbolMatrix.Draw(new Symbol(CharacterFill, Color), v, matrix);
                }
            }

            for (var x = pos.X / delay; x < (pos.X + scale.X) / delay; x++)
            {
                for (var y = pos.Y / delay; y < (pos.Y + scale.Y) / delay; y++)
                {
                    if (
                        Math.Abs(x - pos.X / delay) < delay / 2f ||
                        Math.Abs(x - (pos.X + scale.X) / delay) < delay ||
                        Math.Abs(y - pos.Y / delay) < delay / 2f ||
                        Math.Abs(y - (pos.Y + scale.Y) / delay) < delay / 2f
                    )
                    {
                        SymbolMatrix.Draw(new Symbol(CharacterBorders, Color),
                            SymbolMatrix.WorldToSymbolMatrixPosition(new Vector2(x, y) * delay, _camera, true), matrix);
                    }
                }
            }

            var ss = new SymbolString(Text);
            SymbolString.PlaceAt(matrix, ss, SymbolMatrix.WorldToSymbolMatrixPosition(
                pos + scale / 2f, _camera, true) - ss.Bounds.Extends.Multiply(new Vector2(1, 0.25f)));
        }
    }
}