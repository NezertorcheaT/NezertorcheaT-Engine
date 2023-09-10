using System;
using System.Numerics;
using Engine.Components;

namespace Engine
{
    public static class Renderer
    {
        public static void DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol, Camera? camera,
            GameConfigData gcd, Vector2 cameraPosition)
        {
            if (camera == null) return;
            worldPos = new Vector2(worldPos.X, -worldPos.Y);
            var pos = worldPos - cameraPosition + camera.Offset.Da2V2();

            if (pos.X < 0 || pos.X > gcd.WIDTH || pos.Y < 0 || pos.Y > gcd.HEIGHT) return;
            symbolMatrix.Draw(symbol, pos);
        }
    }

    public class Drawer : Component, IRenderer
    {
        public char Character = ' ';

        public Drawer(GameObject gameObject) : base(gameObject)
        {
        }

        public void OnDraw(SymbolMatrix matrix, GameConfigData? config = null)
        {
            var cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);

            Renderer.DrawSymbol(matrix, transform.Position,
                new Symbol {Character = Character, Color = ConsoleColor.White}, cam, config ?? GameConfig.GetData(),
                cam.transform.Position);
        }
    }
}