
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlimeLab.Entities.Player
{
    public sealed class PlayerEntity : Entity
    {
        public bool IsDead => this.nextPlayerScale == Vector2.Zero;
        public float PlayerRadius => this.playerRadius * this.Scale.X / 2;

        public Vector2 NextPlayerPosition { get => this.nextPlayerPosition; set => this.nextPlayerPosition = value; }
        public Vector2 NextPlayerScale { get => this.nextPlayerScale; set => this.nextPlayerScale = value; }

        // PLAYER STATUS
        private readonly float playerSpeed = 300f;
        private readonly float playerRadius = 32f;

        // PLAYER WORLD MAP
        private Vector2 nextPlayerPosition;
        private Vector2 nextPlayerScale;

        // PLAYER TEXTURE ANIMATIONS
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        public PlayerEntity(Core core) : base(core)
        {

        }

        //=========================//

        public override void Initialize()
        {
            this.Scale = Vector2.One;
            this.Position = new(this.Core.GraphicsDeviceManager.PreferredBackBufferWidth / 2, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight / 2);

            this.nextPlayerPosition = this.Position;
            this.nextPlayerScale = this.Scale;
        }

        public override void Update(GameTime gameTime)
        {
            PlayerScaleUpdate(gameTime);
            PlayerPositionUpdate(gameTime);
            PlayerControlsUpdate(gameTime);

            LockPlayerInMap();
            LockPlayerScale();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.PlayerSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.PlayerSheetTextures[this.currentState].Width / 2, this.Core.PlayerSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void PlayerScaleUpdate(GameTime gameTime)
        {
            this.Scale = Vector2.Lerp(this.Scale, this.nextPlayerScale, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void PlayerPositionUpdate(GameTime gameTime)
        {
            this.Position = Vector2.Lerp(this.Position, this.nextPlayerPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void PlayerControlsUpdate(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                this.nextPlayerPosition.Y -= this.playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                this.nextPlayerPosition.Y += this.playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                this.nextPlayerPosition.X -= this.playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                this.nextPlayerPosition.X += this.playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void LockPlayerInMap()
        {
            Vector2 tempPos = Vector2.One;

            tempPos.X = MathHelper.Clamp(this.Position.X, this.Scale.X * 32 / 2, this.Core.GraphicsDeviceManager.PreferredBackBufferWidth - (this.Scale.X * 32 / 2));
            tempPos.Y = MathHelper.Clamp(this.Position.Y, this.Scale.Y * 32 / 2, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight - (this.Scale.Y * 32 / 2));

            this.Position = tempPos;

            this.nextPlayerPosition.X = MathHelper.Clamp(this.nextPlayerPosition.X, this.Scale.X * 32 / 2, this.Core.GraphicsDeviceManager.PreferredBackBufferWidth - (this.Scale.X * 32 / 2));
            this.nextPlayerPosition.Y = MathHelper.Clamp(this.nextPlayerPosition.Y, this.Scale.Y * 32 / 2, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight - (this.Scale.Y * 32 / 2));
        }

        private void LockPlayerScale()
        {
            if (this.nextPlayerScale.X < 0.5f)
            {
                this.nextPlayerScale = Vector2.Zero;
            }

            this.Scale = Vector2.Clamp(this.Scale, Vector2.Zero, new Vector2(float.MaxValue));
            this.nextPlayerScale = Vector2.Clamp(this.nextPlayerScale, Vector2.Zero, new Vector2(float.MaxValue));
        }

        //=========================//

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.changeStateCurrentTime < this.changeStateTime)
            {
                this.changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.PlayerSheetTextures.Length - 1)
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
