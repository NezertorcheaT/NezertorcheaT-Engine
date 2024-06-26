﻿using System;
using Engine.Render.Symbols;
using Engine.Scene;

namespace Engine.Components.ConsoleRenderers
{
    public class ConsoleRenderer : Component, IRenderer
    {
        public char Character = ' ';
        public ConsoleColor Color = ConsoleColor.White;

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            if (!ActiveAndEnabled) return;
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);
            var worldPos = transform.Position;

            if (SymbolMatrix.WorldToSymbolMatrixPosition(ref worldPos, cam))
                SymbolMatrix.Draw(new Symbol {Character = Character, Color = Color}, worldPos, matrix);
        }
    }
}