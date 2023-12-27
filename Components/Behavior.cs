﻿using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public abstract class Behavior : Component, IComponentStart, IComponentUpdate
    {
        void IComponentStart.Start()
        {
            Start();
            FixedCycle();
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        private async void FixedCycle()
        {
            for (;;)
            {
                await Task.Delay((int) (GameConfig.GetData().FIXED_REPETITIONS * 1000));
                if (!enabled) continue;
                try
                {
                    FixedUpdate();
                }
                catch (Exception e)
                {
                    Logger.Log(e, "Fixed update error");
                }
            }
        }

        public virtual void FixedUpdate()
        {
        }
    }
}