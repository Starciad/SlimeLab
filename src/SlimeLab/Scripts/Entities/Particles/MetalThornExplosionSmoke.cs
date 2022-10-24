using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public class MetalThornExplosionSmoke : Entity
    {
        private Random _random;

        // POSITION
        private Vector2 position;

        // ANIMATION
        private Texture2D[] particleSheet;
        private int currentState;

        private readonly float nextStateTime = .1f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            _random = new();

            particleSheet = new Texture2D[]
            {
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke1"),
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke2"),
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke3"),
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalRhotnExplosionSmoke4"),
            };

            position = InstancePosition;
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            PositionUpdate();
            if (currentState == particleSheet.Length - 1)
            {
                EntityManager.DestroyEntity(this);
            }
        }

        private void PositionUpdate()
        {
            position.X += _random.Next(-3, 3);
            position.Y -= _random.Next(1, 4);
        }

        //========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(particleSheet[currentState],
                             position,
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
