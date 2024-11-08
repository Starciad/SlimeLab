using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities;
using SlimeLab.Objects;

using System;
using System.Collections.Generic;

namespace SlimeLab.Managers
{
    public sealed class EntityManager : GameObject
    {
        public Entity[] Entities => this.instantiatedEntities.ToArray();

        private readonly List<Entity> instantiatedEntities = new();

        public EntityManager(Core core) : base(core)
        {

        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                this.instantiatedEntities[i].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                this.instantiatedEntities[i].Draw(gameTime, spriteBatch);
            }
        }

        public T InstantiateEntity<T>() where T : Entity
        {
            return InstantiateEntity<T>(Vector2.Zero);
        }

        public T InstantiateEntity<T>(Vector2 position) where T : Entity
        {
            Entity entity = (Entity)Activator.CreateInstance(typeof(T), new object[] { this.Core });
            entity.Position = position;
            entity.Initialize();

            this.instantiatedEntities.Add(entity);
            return (T)entity;
        }

        public T GetEntity<T>() where T : Entity
        {
            return (T)(object)this.instantiatedEntities.Find(x => x.GetType() == typeof(T));
        }

        public void DestroyEntity(Entity entity)
        {
            _ = this.instantiatedEntities.Remove(entity);
            entity.Destroy();
        }

        public void ClearEntitites()
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                this.instantiatedEntities[i].Destroy();
            }

            this.instantiatedEntities.Clear();
        }
    }
}
