using System;
using System.Numerics;
using Engine.Components;

namespace Engine.ConsoleRenderers
{
    public class ConsoleRenderer : Component, IRenderer
    {
        public char Character = ' ';

        void IRenderer.OnDraw(SymbolMatrix matrix)
        {
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);

            DrawSymbol(
                matrix,
                transform.Position,
                new Symbol {Character = Character, Color = ConsoleColor.White},
                cam
            );
        }

        private static void DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol, Camera? camera)
        {
            if (camera == null) return;
            var camPos = camera.transform.Position;
            camPos = new Vector2(camPos.X, -camPos.Y);
            worldPos = new Vector2(worldPos.X, -worldPos.Y);

            var pos = worldPos - camPos + camera.Offset.Da2V2();
            pos.X *= Symbol.FiveByEight;

            if (pos.X < 0 || pos.X > GameConfig.Data.WIDTH || pos.Y < 0 || pos.Y > GameConfig.Data.HEIGHT) return;
            symbolMatrix.Draw(symbol, pos);
        }
    }
}