using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab.Objects
{
    public abstract class GameObject
    {
        protected Core Core { get; private set; }

        public GameObject(Core core)
        {
            this.Core = core;
        }

        public virtual void Initialize() { return; }
        public virtual void Update(GameTime gameTime) { return; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
    }
}
