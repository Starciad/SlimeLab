using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Objects;

using System.Text;

namespace SlimeLab.Managers
{
    public sealed class ScoreManager : GameObject
    {
        public int Score { get; set; }

        public SpriteFont DefaultFont { get; set; }

        public ScoreManager(Core core) : base(core)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            StringBuilder content = new(this.Score.ToString("00"));
            Vector2 stringSize = this.DefaultFont.MeasureString(content.ToString());

            spriteBatch.DrawString(this.DefaultFont, content.ToString(), new Vector2((this.Core.GraphicsDeviceManager.PreferredBackBufferWidth / 2) - stringSize.X, 32), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
