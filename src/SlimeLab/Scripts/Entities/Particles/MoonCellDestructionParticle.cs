using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public class MoonCellDestructionParticle : Entity
    {
        // ANIMATION
        private Texture2D[] particleSheet;
        private int currentState;

        private float nextStateTime = .05f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            particleSheet = new Texture2D[]
            {
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion1"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion2"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion3"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion4"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion5"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion6"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion7"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion8"),
                content.Load<Texture2D>(@"Sprites\Particles\MoonCell\MoonCellExplosion9"),
            };
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            if (currentState == particleSheet.Length - 1)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        //========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(particleSheet[currentState],
                             InstancePosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(particleSheet[currentState].Width / 2, particleSheet[currentState].Height / 2),
                             Vector2.One,
                             SpriteEffects.None,
                             0f);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (nextStateCurrentTime < nextStateTime)
            {
                nextStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (currentState < particleSheet.Length - 1)
                    currentState++;
                else
                    currentState = 0;

                nextStateCurrentTime = 0;
            }
        }
    }
}
