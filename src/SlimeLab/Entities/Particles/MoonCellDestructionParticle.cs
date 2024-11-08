
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Managers;

namespace SlimeLab.Entities.Particles
{
    public class MoonCellDestructionParticle : Entity
    {
        private Core _core;

        // ANIMATION
        private int currentState;

        private readonly float nextStateTime = .05f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.currentState == this._core.MoonCellSheetTextures.Length - 1)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        //========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.MoonCellSheetTextures[this.currentState],
                             this.InstancePosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.MoonCellSheetTextures[this.currentState].Width / 2, this._core.MoonCellSheetTextures[this.currentState].Height / 2),
                             Vector2.One,
                             SpriteEffects.None,
                             0f);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.nextStateCurrentTime < this.nextStateTime)
            {
                this.nextStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this._core.MoonCellSheetTextures.Length - 1)
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
