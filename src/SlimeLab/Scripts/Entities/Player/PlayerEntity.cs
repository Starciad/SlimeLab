
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlimeLab
{
    public class PlayerEntity : Entity
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        public bool IsDead { get { return nextPlayerScale == Vector2.Zero; } }
        public float PlayerRadius { get { return playerRadius * playerScale.X / 2; } }

        public Vector2 PlayerPosition { get { return playerPosition; } }
        public Vector2 PlayerScale { get { return playerScale; } }

        public Vector2 NextPlayerPosition { get { return nextPlayerPosition; } set { nextPlayerPosition = value; } }
        public Vector2 NextPlayerScale { get { return nextPlayerScale; } set { nextPlayerScale = value; } }


        // PLAYER STATUS
        private readonly float playerSpeed = 300f;
        private readonly float playerRadius = 32f;

        // PLAYER WORLD MAP
        private Vector2 playerPosition;
        private Vector2 playerScale;

        private Vector2 nextPlayerPosition;
        private Vector2 nextPlayerScale;

        // PLAYER TEXTURE ANIMATIONS
        private Texture2D[] playerSheetTextures;
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        //=========================//

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _content = content;

            playerSheetTextures = new Texture2D[]
            {
                _content.Load<Texture2D>(@"Sprites\Player\Slime1"),
                _content.Load<Texture2D>(@"Sprites\Player\Slime2"),
                _content.Load<Texture2D>(@"Sprites\Player\Slime3"),
                _content.Load<Texture2D>(@"Sprites\Player\Slime4"),
            };

            playerScale = new(1, 1);
            playerPosition = new(_graphics.PreferredBackBufferWidth / 2,
                                 _graphics.PreferredBackBufferHeight / 2);

            nextPlayerPosition = playerPosition;
            nextPlayerScale = playerScale;
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
            playerScale = Vector2.Lerp(playerScale, nextPlayerScale, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        private void PlayerPositionUpdate(GameTime gameTime)
        {
            playerPosition = Vector2.Lerp(playerPosition, nextPlayerPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        private void PlayerControlsUpdate(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                nextPlayerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                nextPlayerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                nextPlayerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                nextPlayerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void LockPlayerInMap()
        {
            playerPosition.X = MathHelper.Clamp(playerPosition.X, (playerScale.X * 32) / 2, _graphics.PreferredBackBufferWidth - ((playerScale.X * 32) / 2));
            playerPosition.Y = MathHelper.Clamp(playerPosition.Y, (playerScale.Y * 32) / 2, _graphics.PreferredBackBufferHeight - ((playerScale.Y * 32) / 2));

            nextPlayerPosition.X = MathHelper.Clamp(nextPlayerPosition.X, (playerScale.X * 32) / 2, _graphics.PreferredBackBufferWidth - ((playerScale.X * 32) / 2));
            nextPlayerPosition.Y = MathHelper.Clamp(nextPlayerPosition.Y, (playerScale.Y * 32) / 2, _graphics.PreferredBackBufferHeight - ((playerScale.Y * 32) / 2));
        }
        private void LockPlayerScale()
        {
            if (nextPlayerScale.X < 0.5f)
                nextPlayerScale = Vector2.Zero;

            playerScale = Vector2.Clamp(playerScale, Vector2.Zero, new Vector2(float.MaxValue));
            nextPlayerScale = Vector2.Clamp(nextPlayerScale, Vector2.Zero, new Vector2(float.MaxValue));
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(playerSheetTextures[currentState],
                             playerPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(playerSheetTextures[currentState].Width / 2, playerSheetTextures[currentState].Height / 2),
                             playerScale,
                             SpriteEffects.None,
                             0f);
        }
        private void AnimationUpdate(GameTime gameTime)
        {
            if (changeStateCurrentTime < changeStateTime)
            {
                changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (currentState < playerSheetTextures.Length - 1)
                    currentState++;
                else
                    currentState = 0;

                changeStateCurrentTime = 0;
            }
        }
    }
}
