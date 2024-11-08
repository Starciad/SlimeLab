
using Microsoft.Xna.Framework;

namespace SlimeLab.Managers
{
    public static class TickManager
    {
        public delegate void Ticked();
        public static event Ticked OnTicked;

        private static readonly float TickTime = 1f;
        private static float CurrentTickTime = 0f;

        public static void UpdateTick(GameTime gameTime)
        {
            if (CurrentTickTime < TickTime)
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
