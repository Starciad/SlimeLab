using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.Player;
using SlimeLab.Objects;

using System.Text;

namespace SlimeLab.Managers
{
    public sealed class GameManager : GameObject
    {
        public bool IsGameEnded { get; set; }
        public SpriteFont DefaultFont { get; set; }

        private PlayerEntity player;

        public GameManager(Core core) : base(core)
        {

        }

        public override void Initialize()
        {
            this.player = this.Core.EntityManager.GetEntity<PlayerEntity>();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.IsGameEnded && this.player.IsDead)
            {
                this.IsGameEnded = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsGameEnded)
            {
                return;
            }

            StringBuilder content = new("Press R to reset!");
            Vector2 stringSize = this.DefaultFont.MeasureString(content.ToString());

            spriteBatch.DrawString(this.DefaultFont, content.ToString(), new Vector2((this.Core.GraphicsDeviceManager.PreferredBackBufferWidth / 2) - stringSize.X, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight - 128), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
