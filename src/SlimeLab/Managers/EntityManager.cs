using Microsoft.Xna.Framework;
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
                instantiatedEntities[i].Update(gameTime);
            }
        }
        public static void DrawEntities(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < instantiatedEntities.Count; i++)
            {
                instantiatedEntities[i].Draw(gameTime, spriteBatch);
            }
        }

        //=================================//

        public static T InstantiateEntity<T>(Core core) where T : Entity
        {
            return InstantiateEntity<T>(core, Vector2.Zero);
        }
        public static T InstantiateEntity<T>(Core core, Vector2 position) where T : Entity
        {
            Entity entity = (Entity)Activator.CreateInstance(typeof(T), new object[] { core });
            entity.Position = position;
            entity.Startup();

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
            entity.Destroy();
        }

        public static void ClearEntitites()
        {
            for (int i = 0; i < instantiatedEntities.Count; i++)
            {
                instantiatedEntities[i].Destroy();
            }

            instantiatedEntities.Clear();
        }
    }
}
