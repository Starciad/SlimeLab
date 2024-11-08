
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Managers;

namespace SlimeLab.Entities.Particles
{
    public class MetalThornExplosion : Entity
    {
        public int Direction { get; set; }

        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private Core _core;

        // POSITION
        private Vector2 position;

        // STATUS
        private readonly float speed = 550f;

        // SMOKE
        private readonly float smokeSpawnTime = .01f;
        private float currentSmokeSpawnTime = 0f;

        // ANIMATION
        private int currentState;

        private readonly float nextStateTime = .08f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
            this._graphics = graphics;
            this._content = content;

            this.position = this.InstancePosition;
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            PositionUpdate(gameTime);
            SmokeUpdate(gameTime);

            if (this.position.X > this._graphics.PreferredBackBufferWidth + 128 || this.position.X < -128 ||
               this.position.Y > this._graphics.PreferredBackBufferHeight + 128 || this.position.Y < -128)
            {
                EntityManager.DestroyEntity(this);
            }
        }
        private void PositionUpdate(GameTime gameTime)
        {
            switch (this.Direction)
            {
                case 1: // UP - LEFT
                    this.position.X -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.position.Y -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 2: // UP - RIGHT
                    this.position.X += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.position.Y -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 3: // DOWN - LEFT
                    this.position.X -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.position.Y += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 4: // DOWN - RIGHT
                    this.position.X += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.position.Y += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                _ = EntityManager.InstantiateEntity<MetalThornExplosionSmoke>(this._core, this._graphics, this._content, this.position);

                this.currentSmokeSpawnTime = 0;
            }
        }

        //========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.ExplosionSheetTextures[this.currentState],
                             this.position,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.ExplosionSheetTextures[this.currentState].Width / 2, this._core.ExplosionSheetTextures[this.currentState].Height / 2),
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
                if (this.currentState < this._core.ExplosionSheetTextures.Length - 1)
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
