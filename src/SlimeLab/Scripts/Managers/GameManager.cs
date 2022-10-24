using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SlimeLab
{
    public static class GameManager
    {
        public static bool IsGameEnded { get; set; }
        public static SpriteFont DefaultFont { get; set; }

        private static PlayerEntity player;

        public static void Startup()
        {
            player = EntityManager.GetEntity<PlayerEntity>();
        }

        public static void Update()
        {
            if (!IsGameEnded && player.IsDead)
            {
                IsGameEnded = true;
            }
        }

        public static void Render(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (!IsGameEnded)
                return;

            StringBuilder content = new("Press R to reset!");
            Vector2 stringSize = DefaultFont.MeasureString(content.ToString());

            spriteBatch.DrawString(DefaultFont, content.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - stringSize.X, graphics.PreferredBackBufferHeight - 128), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}
