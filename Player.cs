using System;
using System.Numerics;
using Engine.Components;

namespace Engine
{
    public class Player : Behavior
    {
        public override void Start()
        {
            foreach (var v in GameObject.FindAllTypes<Transform>(gameObject.hierarchy))
            {
                Logger.Log(v,"ass");
            }
        }

        public override void Update()
        {
        }
    }
}