
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab.Entities.Particles
{
    public class MoonCellDestructionParticle : Entity
    {
        // ANIMATION
        private int currentState;

        private readonly float nextStateTime = .05f;
        private float nextStateCurrentTime = 0f;

        public MoonCellDestructionParticle(Core core) : base(core)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (this.currentState == this.Core.MoonCellSheetTextures.Length - 1)
            {
                this.Core.EntityManager.DestroyEntity(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.MoonCellSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.MoonCellSheetTextures[this.currentState].Width / 2, this.Core.MoonCellSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.nextStateCurrentTime < this.nextStateTime)
            {
                this.nextStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.MoonCellSheetTextures.Length - 1)
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
