using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab.Entities
{
    public abstract class Entity
    {
        protected Core Core { get; private set; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public Entity(Core core)
        {
            this.Core = core;

            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
        }

        public virtual void Startup() { return; }
        public virtual void Update(GameTime gameTime) { return; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
        public virtual void Destroy() { return; }
    }
}
