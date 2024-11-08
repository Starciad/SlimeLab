
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlimeLab.Entities.Player
{
    public class PlayerEntity : Entity
    {
        public bool IsDead => this.nextPlayerScale == Vector2.Zero;
        public float PlayerRadius => this.playerRadius * this.playerScale.X / 2;

        public Vector2 PlayerPosition => this.playerPosition;
        public Vector2 PlayerScale => this.playerScale;

        public Vector2 NextPlayerPosition { get => this.nextPlayerPosition; set => this.nextPlayerPosition = value; }
        public Vector2 NextPlayerScale { get => this.nextPlayerScale; set => this.nextPlayerScale = value; }

        private Core _core;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        // PLAYER STATUS
        private readonly float playerSpeed = 300f;
        private readonly float playerRadius = 32f;

        // PLAYER WORLD MAP
        private Vector2 playerPosition;
        private Vector2 playerScale;

        private Vector2 nextPlayerPosition;
        private Vector2 nextPlayerScale;

        // PLAYER TEXTURE ANIMATIONS
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        //=========================//

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
            this._graphics = graphics;
            this._content = content;

            this.playerScale = new(1, 1);
            this.playerPosition = new(this._graphics.PreferredBackBufferWidth / 2,
                                 this._graphics.PreferredBackBufferHeight / 2);

            this.nextPlayerPosition = this.playerPosition;
            this.nextPlayerScale = this.playerScale;
        }

        //=========================//

        protected override void OnStartup()
        {

        }

        //=========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            PlayerScaleUpdate(gameTime);
            PlayerPositionUpdate(gameTime);
            PlayerControlsUpdate(gameTime);

            LockPlayerInMap();
            LockPlayerScale();
        }
        private void PlayerScaleUpdate(GameTime gameTime)
        {
            this.playerScale = Vector2.Lerp(this.playerScale, this.nextPlayerScale, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        private void PlayerPositionUpdate(GameTime gameTime)
        {
            this.playerPosition = Vector2.Lerp(this.playerPosition, this.nextPlayerPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
            this.playerPosition.X = MathHelper.Clamp(this.playerPosition.X, this.playerScale.X * 32 / 2, this._graphics.PreferredBackBufferWidth - (this.playerScale.X * 32 / 2));
            this.playerPosition.Y = MathHelper.Clamp(this.playerPosition.Y, this.playerScale.Y * 32 / 2, this._graphics.PreferredBackBufferHeight - (this.playerScale.Y * 32 / 2));

            this.nextPlayerPosition.X = MathHelper.Clamp(this.nextPlayerPosition.X, this.playerScale.X * 32 / 2, this._graphics.PreferredBackBufferWidth - (this.playerScale.X * 32 / 2));
            this.nextPlayerPosition.Y = MathHelper.Clamp(this.nextPlayerPosition.Y, this.playerScale.Y * 32 / 2, this._graphics.PreferredBackBufferHeight - (this.playerScale.Y * 32 / 2));
        }
        private void LockPlayerScale()
        {
            if (this.nextPlayerScale.X < 0.5f)
            {
                this.nextPlayerScale = Vector2.Zero;
            }

            this.playerScale = Vector2.Clamp(this.playerScale, Vector2.Zero, new Vector2(float.MaxValue));
            this.nextPlayerScale = Vector2.Clamp(this.nextPlayerScale, Vector2.Zero, new Vector2(float.MaxValue));
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.PlayerSheetTextures[this.currentState],
                             this.playerPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.PlayerSheetTextures[this.currentState].Width / 2, this._core.PlayerSheetTextures[this.currentState].Height / 2),
                             this.playerScale,
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
                if (this.currentState < this._core.PlayerSheetTextures.Length - 1)
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
