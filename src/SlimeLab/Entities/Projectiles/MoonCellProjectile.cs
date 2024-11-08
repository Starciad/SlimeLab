using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.MetalThorn;
using SlimeLab.Entities.Particles;

using System.Linq;

namespace SlimeLab.Entities.Projectiles
{
    public sealed class MoonCellProjectile : Entity
    {
        private readonly float speed = 200f;
        private readonly float readius = 16f;

        private Vector2 targetDirection;
        private bool hasTarget;

        public MoonCellProjectile(Core core) : base(core)
        {

        }

        public override void Initialize()
        {
            FindNearestTarget();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.hasTarget)
            {
                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveStep = this.targetDirection * this.speed * deltaTime;
            this.Position += moveStep;

            foreach (MetalThornEntity enemy in this.Core.EntityManager.Entities.Where(x => x is MetalThornEntity).Cast<MetalThornEntity>())
            {
                if (Vector2.Distance(this.Position, enemy.Position) < this.readius)
                {
                    this.Core.EntityManager.DestroyEntity(enemy);
                    this.Core.EntityManager.DestroyEntity(this);
                    return;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Core.ProjectileTexture, this.Position, null, Color.White, 0f, new Vector2(this.Core.ProjectileTexture.Width / 2, this.Core.ProjectileTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public override void Destroy()
        {
            for (int i = 0; i < 4; i++)
            {
                MetalThornExplosion explosion = this.Core.EntityManager.InstantiateEntity<MetalThornExplosion>(this.Position);
                explosion.Direction = i + 1;
            }
        }

        private void FindNearestTarget()
        {
            Entity[] enemies = this.Core.EntityManager.Entities.Where(x => x is MetalThornEntity).Cast<MetalThornEntity>().ToArray();
            Entity nearestEnemy = enemies
                .OrderBy(e => Vector2.Distance(this.Position, e.Position))
                .FirstOrDefault();

            if (nearestEnemy != null)
            {
                this.targetDirection = Vector2.Normalize(nearestEnemy.Position - this.Position);
                this.hasTarget = true;
            }
        }
    }
}
