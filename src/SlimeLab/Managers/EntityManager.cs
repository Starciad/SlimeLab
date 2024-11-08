using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities;

using System;
using System.Collections.Generic;

namespace SlimeLab.Managers
{
    public static class EntityManager
    {
        private static readonly List<Entity> instantiatedEntities = new();

        //=================================//

        public static void UpdateEntities(GameTime gameTime)
        {
            for (int i = 0; i < instantiatedEntities.Count; i++)
            {
                instantiatedEntities[i].UpdateEntity(gameTime);
            }
        }
        public static void RenderEntities(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < instantiatedEntities.Count; i++)
            {
                instantiatedEntities[i].RenderEntity(gameTime, spriteBatch);
            }
        }

        //=================================//

        public static T InstantiateEntity<T>(Core core, GraphicsDeviceManager graphics, ContentManager content) where T : Entity
        {
            return InstantiateEntity<T>(core, graphics, content, Vector2.Zero);
        }
        public static T InstantiateEntity<T>(Core core, GraphicsDeviceManager graphics, ContentManager content, Vector2 position) where T : Entity
        {
            Entity entity = Activator.CreateInstance<T>();

            entity.InstantiateEntity(core, graphics, content, position);
            entity.StartupEntity();

            instantiatedEntities.Add(entity);
            return (T)entity;
        }

        public static T GetEntity<T>() where T : Entity
        {
            return (T)(object)instantiatedEntities.Find(x => x.GetType() == typeof(T));
        }

        public static void DestroyEntity(Entity entity)
        {
            _ = instantiatedEntities.Remove(entity);
            entity.DestroyEntity();
        }

        public static void ClearEntitites()
        {
            for (int i = 0; i < instantiatedEntities.Count; i++)
            {
                instantiatedEntities[i].DestroyEntity();
            }

            instantiatedEntities.Clear();
        }
    }
}
