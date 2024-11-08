
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Managers;

namespace SlimeLab.Entities.Particles
{
    public class MetalThornExplosion : Entity
    {
        public int Direction { get; set; }

        // STATUS
        private readonly float speed = 550f;

        // SMOKE
        private readonly float smokeSpawnTime = .01f;
        private float currentSmokeSpawnTime = 0f;

        // ANIMATION
        private int currentState;

        private readonly float nextStateTime = .08f;
        private float nextStateCurrentTime = 0f;

        public MetalThornExplosion(Core core) : base(core)
        {

        }

        public override void Update(GameTime gameTime)
        {
            PositionUpdate(gameTime);
            SmokeUpdate(gameTime);

            if (this.Position.X > this.Core.GraphicsDeviceManager.PreferredBackBufferWidth + 128 || this.Position.X < -128 ||
               this.Position.Y > this.Core.GraphicsDeviceManager.PreferredBackBufferHeight + 128 || this.Position.Y < -128)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.ExplosionSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.ExplosionSheetTextures[this.currentState].Width / 2, this.Core.ExplosionSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void PositionUpdate(GameTime gameTime)
        {
            switch (this.Direction)
            {
                case 1: // UP - LEFT
                    this.Position = new(this.Position.X - (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds), this.Position.Y - (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    break;

                case 2: // UP - RIGHT
                    this.Position = new(this.Position.X + (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds), this.Position.Y - (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    break;

                case 3: // DOWN - LEFT
                    this.Position = new(this.Position.X - (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds), this.Position.Y + (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    break;

                case 4: // DOWN - RIGHT
                    this.Position = new(this.Position.X + (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds), this.Position.Y + (this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    break;
            }
        }

        private void SmokeUpdate(GameTime gameTime)
        {
            if (this.currentSmokeSpawnTime < this.smokeSpawnTime)
            {
                this.currentSmokeSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _ = EntityManager.InstantiateEntity<MetalThornExplosionSmoke>(this.Core, this.Position);

                this.currentSmokeSpawnTime = 0;
            }
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.nextStateCurrentTime < this.nextStateTime)
            {
                this.nextStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.ExplosionSheetTextures.Length - 1)
                {
                    this.currentState++;
                }
                else
                {
                    this.currentState = 0;
                }

                this.nextStateCurrentTime = 0;
            }
        }
    }
}
