using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.Particles;
using SlimeLab.Entities.Player;

namespace SlimeLab.Entities.MetalThorn
{
    public class MetalThornEntity : Entity
    {
        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private readonly float metalThornPower = 0.8f;

        // CELL WORLD MAP
        private readonly float metalThornSpeed = 2f;
        private Vector2 nextMetalThornPosition;

        // CELL TEXTURE ANIMATIONS
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        public MetalThornEntity(Core core) : base(core)
        {

        }

        public override void Initialize()
        {
            this.Position = new(this.Core.Random.Next(0, this.Core.GraphicsDeviceManager.PreferredBackBufferWidth), this.Core.Random.Next(0, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight));

            Vector2 tempPos = Vector2.Zero;

            tempPos.X = tempPos.X < this.Core.GraphicsDeviceManager.PreferredBackBufferWidth / 2 ? -100 : tempPos.X = this.Core.GraphicsDeviceManager.PreferredBackBufferWidth + 100;
            tempPos.Y = tempPos.Y < this.Core.GraphicsDeviceManager.PreferredBackBufferHeight / 2 ? tempPos.X = this.Core.GraphicsDeviceManager.PreferredBackBufferWidth + 100 : -100;

            this.Position = tempPos;
            this.nextMetalThornPosition = this.Position;

            this.playerEntity = this.Core.EntityManager.GetEntity<PlayerEntity>();
        }

        public override void Update(GameTime gameTime)
        {
            MoveToPlayerUpdate(gameTime);
            CollisionWithPlayerUpdate();
            MetalThornPositionUpdate(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.MetalThornSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.MetalThornSheetTextures[this.currentState].Width / 2, this.Core.MetalThornSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void MoveToPlayerUpdate(GameTime gameTime)
        {
            if (this.playerEntity.Position.X + this.Core.Random.Next(-1000, 1000) < this.Position.X)
            {
                this.nextMetalThornPosition.X -= this.Core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                this.nextMetalThornPosition.X += this.Core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (this.playerEntity.Position.Y + this.Core.Random.Next(-1000, 1000) < this.Position.Y)
            {
                this.nextMetalThornPosition.Y -= this.Core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                this.nextMetalThornPosition.Y += this.Core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private void CollisionWithPlayerUpdate()
        {
            float distance = Vector2.Distance(this.Position, this.playerEntity.Position);
            if (distance < this.playerEntity.PlayerRadius && !this.collected)
            {
                this.collected = true;
                this.playerEntity.NextPlayerScale -= new Vector2(this.metalThornPower, this.metalThornPower);

                for (int i = 0; i < 4; i++)
                {
                    MetalThornExplosion explosion = this.Core.EntityManager.InstantiateEntity<MetalThornExplosion>(this.Position);
                    explosion.Direction = i + 1;
                }

                this.Core.ExplosionSoundEffect.CreateInstance().Play();
                this.Core.EntityManager.DestroyEntity(this);
            }
        }

        private void MetalThornPositionUpdate(GameTime gameTime)
        {
            this.Position = Vector2.Lerp(this.Position, this.nextMetalThornPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.changeStateCurrentTime < this.changeStateTime)
            {
                this.changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.MetalThornSheetTextures.Length - 1)
                {
                    this.currentState++;
                }
                else
                {
                    this.currentState = 0;
                }

                this.changeStateCurrentTime = 0;
            }
        }
    }
}
