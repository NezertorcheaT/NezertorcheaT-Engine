using System.Numerics;
using Engine.Components;

namespace Engine
{
    public class Drawer : Component, IRenderer
    {
        void IRenderer.DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol, Camera? camera,
            GameConfigData gcd, Vector2 cameraPosition)
        {
            if (camera == null) return;
            worldPos=new Vector2(worldPos.X,-worldPos.Y);
            var pos = worldPos - cameraPosition + camera.Offset.Da2V2();

            if (pos.X < 0 || pos.X > gcd.WIDTH || pos.Y < 0 || pos.Y > gcd.HEIGHT) return;
            symbolMatrix.Draw(symbol, new Size((uint) pos.X, (uint) pos.Y));
        }

        void IRenderer.DrawSymbol(SymbolMatrix symbolMatrix, Vector2 worldPos, Symbol symbol)
        {
            var config = GameConfig.GetData();
            var camera = GameObject.FindObjectByTag("mainCamera", gameObject.hierarchy);

            if (camera == null) return;

            ((IRenderer) this).DrawSymbol(symbolMatrix, worldPos, symbol, camera.GetComponent<Camera>(), config,
                camera.transform.Position);
        }

        public Drawer(GameObject gameObject) : base(gameObject)
        {
        }
    }
}