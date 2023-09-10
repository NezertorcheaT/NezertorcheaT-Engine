using System.Numerics;
using Engine.Components;

namespace Engine
{
    public interface IRenderer : IComponentInit
    {
        void OnDraw(SymbolMatrix matrix,GameConfigData? config=null);
    }
}