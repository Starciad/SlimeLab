using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Managers;

namespace SlimeLab.Entities.Particles
{
    public class MetalThornExplosionSmoke : Entity
    {
        private Core _core;

        // POSITION
        private Vector2 position;

        // ANIMATION
        private int currentState;

        private readonly float nextStateTime = .1f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
            this.position = this.InstancePosition;
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            PositionUpdate();
            if (this.currentState == this._core.ExplosionSmokeSheetTextures.Length - 1)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        private void PositionUpdate()
        {
            this.position.X += this._core.Random.Next(-3, 3);
            this.position.Y -= this._core.Random.Next(1, 4);
        }

        //========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.ExplosionSmokeSheetTextures[this.currentState],
                             this.position,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.ExplosionSmokeSheetTextures[this.currentState].Width / 2, this._core.ExplosionSmokeSheetTextures[this.currentState].Height / 2),
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
                if (this.currentState < this._core.ExplosionSmokeSheetTextures.Length - 1)
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
