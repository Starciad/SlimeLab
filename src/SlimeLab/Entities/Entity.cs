using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab.Entities
{
    public abstract class Entity
    {
        private bool isReady = false;

        protected Vector2 InstancePosition { get; private set; }

        public void InstantiateEntity(Core core, GraphicsDeviceManager graphics, ContentManager content, Vector2 position)
        {
            this.InstancePosition = position;

            OnInstantiate(core, graphics, content);
        }

        public void StartupEntity()
        {
            OnStartup();
            this.isReady = true;
        }

        public void UpdateEntity(GameTime gameTime)
        {
            if (!this.isReady)
            {
                return;
            }

            OnUpdate(gameTime);
        }

        public void RenderEntity(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.isReady)
            {
                return;
            }

            OnRender(gameTime, spriteBatch);
        }

        public void DestroyEntity()
        {
            OnDestroy();
        }

        protected virtual void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content) { }
        protected virtual void OnStartup() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnRender(GameTime gameTime, SpriteBatch spriteBatch) { }
        protected virtual void OnDestroy() { }
    }
}
