using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public static class ScoreManager
    {
        public static int Score { get; set; }

        public static SpriteFont DefaultFont { get; set; }

        public static void RenderScore(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            StringBuilder content = new(Score.ToString("00"));
            Vector2 stringSize = DefaultFont.MeasureString(content.ToString());

            spriteBatch.DrawString(DefaultFont, content.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - stringSize.X, 32), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
