using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlimeLab
{
    public class Startup : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Startup()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            Window.Title = "Slime Lab";
            Window.IsBorderless = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Reset();

            SpriteFont defaultFont = Content.Load<SpriteFont>(@"SpriteFonts\DefaultFont");
            ScoreManager.DefaultFont = defaultFont;
            GameManager.DefaultFont = defaultFont;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R) && GameManager.IsGameEnded)
            {
                Reset();
            }

            EntityManager.UpdateEntities(gameTime);
            TickManager.UpdateTick(gameTime);
            GameManager.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            EntityManager.RenderEntities(gameTime, _spriteBatch);
            ScoreManager.RenderScore(gameTime, _spriteBatch, _graphics);
            GameManager.Render(gameTime, _spriteBatch, _graphics);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Reset()
        {
            EntityManager.ClearEntitites();
            WorldManager.Cancel();

            WorldManager.Startup(_graphics, Content);
            GameManager.Startup();

            ScoreManager.Score = 0;
            GameManager.IsGameEnded = false;
        }
    }
}