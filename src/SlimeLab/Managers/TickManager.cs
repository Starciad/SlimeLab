
using Microsoft.Xna.Framework;

using SlimeLab.Objects;

namespace SlimeLab.Managers
{
    public sealed class TickManager : GameObject
    {
        public event Ticked OnTicked;

        public delegate void Ticked();

        private readonly float tickTime = 1f;
        private float currentTickTime = 0f;

        public TickManager(Core core) : base(core)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (this.currentTickTime < this.tickTime)
            {
                this.currentTickTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                OnTicked?.Invoke();
                this.currentTickTime = 0;
            }
        }
    }
}
