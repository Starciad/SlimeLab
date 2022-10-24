using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public abstract class Entity : IDisposable
    {
        private bool isReady = false;

        protected Vector2 InstancePosition { get; private set; }

        public void InstantiateEntity(GraphicsDeviceManager graphics, ContentManager content, Vector2 position)
        {
            InstancePosition = position;

            OnInstantiate(graphics, content);
        }

        public void StartupEntity()
        {
            OnStartup();
            isReady = true;
        }

        public void UpdateEntity(GameTime gameTime)
        {
            if (!isReady)
                return;

            OnUpdate(gameTime);
        }

        public void RenderEntity(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isReady)
                return;

            OnRender(gameTime, spriteBatch);
        }

        public void DestroyEntity()
        {
            OnDestroy();
            Dispose();
        }

        protected virtual void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content) { }
        protected virtual void OnStartup() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnRender(GameTime gameTime, SpriteBatch spriteBatch) { }
        protected virtual void OnDestroy() { }

        public void Dispose()
        {
            GC.Collect(GC.GetGeneration(this), GCCollectionMode.Forced);
            GC.SuppressFinalize(this);
        }
    }
}
