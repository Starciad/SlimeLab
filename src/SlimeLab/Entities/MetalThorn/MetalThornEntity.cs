using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.Particles;
using SlimeLab.Entities.Player;
using SlimeLab.Managers;

namespace SlimeLab.Entities.MetalThorn
{
    public class MetalThornEntity : Entity
    {
        private Core _core;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private readonly float metalThornPower = 0.8f;

        // CELL WORLD MAP
        private readonly float metalThornSpeed = 2f;
        private Vector2 metalThornPosition;
        private Vector2 metalThornScale;

        private Vector2 nextMetalThornPosition;

        // CELL TEXTURE ANIMATIONS
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        //=========================//

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
            this._graphics = graphics;
            this._content = content;

            this.metalThornScale = new(1, 1);
            this.metalThornPosition = new(this._core.Random.Next(0, this._graphics.PreferredBackBufferWidth),
                                     this._core.Random.Next(0, this._graphics.PreferredBackBufferHeight));

            //metalThornPosition = new(_core.Random.Next(0, _graphics.PreferredBackBufferWidth),
            //         _core.Random.Next(0, _graphics.PreferredBackBufferHeight));

            this.metalThornPosition.X = this.metalThornPosition.X < this._graphics.PreferredBackBufferWidth / 2 ? -100 : this.metalThornPosition.X = this._graphics.PreferredBackBufferWidth + 100;
            this.metalThornPosition.Y = this.metalThornPosition.Y < this._graphics.PreferredBackBufferHeight / 2 ? this.metalThornPosition.X = this._graphics.PreferredBackBufferWidth + 100 : -100;

            this.nextMetalThornPosition = this.metalThornPosition;
        }

        //=========================//

        protected override void OnStartup()
        {
            this.playerEntity = EntityManager.GetEntity<PlayerEntity>();
        }

        //=========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            MoveToPlayerUpdate(gameTime);
            CollisionWithPlayerUpdate();
            MetalThornPositionUpdate(gameTime);
        }

        private void MoveToPlayerUpdate(GameTime gameTime)
        {
            if (this.playerEntity.PlayerPosition.X + this._core.Random.Next(-1000, 1000) < this.metalThornPosition.X)
            {
                this.nextMetalThornPosition.X -= this._core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                this.nextMetalThornPosition.X += this._core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (this.playerEntity.PlayerPosition.Y + this._core.Random.Next(-1000, 1000) < this.metalThornPosition.Y)
            {
                this.nextMetalThornPosition.Y -= this._core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                this.nextMetalThornPosition.Y += this._core.Random.Next(1, 20) + (this.metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        private void CollisionWithPlayerUpdate()
        {
            float distance = Vector2.Distance(this.metalThornPosition, this.playerEntity.PlayerPosition);
            if (distance < this.playerEntity.PlayerRadius && !this.collected)
            {
                this.collected = true;
                this.playerEntity.NextPlayerScale -= new Vector2(this.metalThornPower, this.metalThornPower);

                for (int i = 0; i < 4; i++)
                {
                    MetalThornExplosion explosion = EntityManager.InstantiateEntity<MetalThornExplosion>(this._core, this._graphics, this._content, this.metalThornPosition);
                    explosion.Direction = i + 1;
                }

                this._core.ExplosionSoundEffect.CreateInstance().Play();
                EntityManager.DestroyEntity(this);
            }
        }
        private void MetalThornPositionUpdate(GameTime gameTime)
        {
            this.metalThornPosition = Vector2.Lerp(this.metalThornPosition, this.nextMetalThornPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.MetalThornSheetTextures[this.currentState],
                             this.metalThornPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.MetalThornSheetTextures[this.currentState].Width / 2, this._core.MetalThornSheetTextures[this.currentState].Height / 2),
                             this.metalThornScale,
                             SpriteEffects.None,
                             0f);
        }
        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.changeStateCurrentTime < this.changeStateTime)
            {
                this.changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this._core.MetalThornSheetTextures.Length - 1)
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
