using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SlimeLab.Managers;

using System;

namespace SlimeLab
{
    public class Core : Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this._graphics;

        public Texture2D[] PlayerSheetTextures => this.playerSheetTextures;
        public Texture2D[] MetalThornSheetTextures => this.metalThornSheetTextures;
        public Texture2D[] CellSheetTextures => this.cellSheetTextures;
        public Texture2D[] ExplosionSheetTextures => this.explosionSheetTextures;
        public Texture2D[] ExplosionSmokeSheetTextures => this.explosionSmokeSheetTextures;
        public Texture2D[] MoonCellSheetTextures => this.moonCellSheetTextures;

        public SoundEffect ExplosionSoundEffect => this.explosionSoundEffect;
        public SoundEffect CollectSoundEffect => this.collectSoundEffect;

        public Random Random => this.random;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D[] playerSheetTextures;
        private Texture2D[] metalThornSheetTextures;
        private Texture2D[] cellSheetTextures;
        private Texture2D[] explosionSheetTextures;
        private Texture2D[] explosionSmokeSheetTextures;
        private Texture2D[] moonCellSheetTextures;

        private SoundEffect explosionSoundEffect;
        private SoundEffect collectSoundEffect;

        private readonly Random random = new();

        public Core()
        {
            this._graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.Window.AllowAltF4 = true;
            this.Window.AllowUserResizing = true;
            this.Window.Title = "Slime Lab";
            this.Window.IsBorderless = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);

            Reset();

            SpriteFont defaultFont = this.Content.Load<SpriteFont>(@"SpriteFonts\DefaultFont");
            ScoreManager.DefaultFont = defaultFont;
            GameManager.DefaultFont = defaultFont;

            this.playerSheetTextures = new Texture2D[]
{
                this.Content.Load<Texture2D>(@"Sprites\Player\Slime1"),
                this.Content.Load<Texture2D>(@"Sprites\Player\Slime2"),
                this.Content.Load<Texture2D>(@"Sprites\Player\Slime3"),
                this.Content.Load<Texture2D>(@"Sprites\Player\Slime4"),
            };

            this.metalThornSheetTextures = new Texture2D[]
{
                this.Content.Load<Texture2D>(@"Sprites\MetalThorn\MetalThorn1"),
                this.Content.Load<Texture2D>(@"Sprites\MetalThorn\MetalThorn2"),
            };

            this.cellSheetTextures = new Texture2D[]
            {
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell1"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell2"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell3"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell4"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell5"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell6"),
                this.Content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell7"),
            };

            this.explosionSheetTextures = new Texture2D[]
            {
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion1"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion2"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion3"),
            };

            this.explosionSmokeSheetTextures = new Texture2D[]
            {
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke1"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke2"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke3"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke4"),
            };

            this.moonCellSheetTextures = new Texture2D[]
            {
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion1"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion2"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion3"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion4"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion5"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion6"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion7"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion8"),
                this.Content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion9"),
            };

            this.explosionSoundEffect = this.Content.Load<SoundEffect>(@"Sounds\bullet-bill");
            this.collectSoundEffect = this.Content.Load<SoundEffect>(@"Sounds\coin");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

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
            this.GraphicsDevice.Clear(Color.White);

            this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            EntityManager.DrawEntities(gameTime, this._spriteBatch);
            ScoreManager.DrawScore(gameTime, this._spriteBatch, this._graphics);
            GameManager.Render(gameTime, this._spriteBatch, this._graphics);
            this._spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Reset()
        {
            EntityManager.ClearEntitites();
            WorldManager.Cancel();

            WorldManager.Startup(this);
            GameManager.Startup();

            ScoreManager.Score = 0;
            GameManager.IsGameEnded = false;
        }
    }
}