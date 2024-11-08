using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Managers;

namespace SlimeLab.Entities.Particles
{
    public class MetalThornExplosionSmoke : Entity
    {
        // ANIMATION
        private int currentState;
        private readonly float nextStateTime = .1f;
        private float nextStateCurrentTime = 0f;

        public MetalThornExplosionSmoke(Core core) : base(core)
        {

        }

        public override void Update(GameTime gameTime)
        {
            PositionUpdate();
            if (this.currentState == this.Core.ExplosionSmokeSheetTextures.Length - 1)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.ExplosionSmokeSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.ExplosionSmokeSheetTextures[this.currentState].Width / 2, this.Core.ExplosionSmokeSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void PositionUpdate()
        {
            this.Position = new(this.Position.X + this.Core.Random.Next(-3, 3), this.Position.Y - this.Core.Random.Next(1, 4));
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.nextStateCurrentTime < this.nextStateTime)
            {
                this.nextStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.ExplosionSmokeSheetTextures.Length - 1)
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
