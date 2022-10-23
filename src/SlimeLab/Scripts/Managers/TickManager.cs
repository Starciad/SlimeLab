using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace SlimeLab
{
    public static class TickManager
    {
        public delegate void Ticked();
        public static event Ticked OnTicked;

        private static float TickTime = 1f;
        private static float CurrentTickTime = 0f;

        public static void UpdateTick(GameTime gameTime)
        {
            if(CurrentTickTime < TickTime)
            {
                CurrentTickTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                OnTicked?.Invoke();
                CurrentTickTime = 0;
            }
        }
    }
}
