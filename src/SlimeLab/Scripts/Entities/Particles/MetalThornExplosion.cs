
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public class MetalThornExplosion : Entity
    {
        public int Direction { get; set; }

        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        // POSITION
        private Vector2 position;

        // STATUS
        private readonly float speed = 550f;

        // SMOKE
        private readonly float smokeSpawnTime = .01f;
        private float currentSmokeSpawnTime = 0f;

        // ANIMATION
        private Texture2D[] particleSheet;
        private int currentState;

        private readonly float nextStateTime = .08f;
        private float nextStateCurrentTime = 0f;

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _content = content;

            particleSheet = new Texture2D[]
            {
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion1"),
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion2"),
                content.Load<Texture2D>(@"Sprites\Particles\MetalThorn\MetalThornExplosion3"),
            };

            position = InstancePosition;
        }

        protected override void OnStartup()
        {

        }

        //========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            PositionUpdate(gameTime);
            SmokeUpdate(gameTime);

            if (position.X > _graphics.PreferredBackBufferWidth + 128 || position.X < -128 ||
               position.Y > _graphics.PreferredBackBufferHeight + 128 || position.Y < -128)
            {
                EntityManager.DestroyEntity(this);
            }
        }
        private void PositionUpdate(GameTime gameTime)
        {
            switch (Direction)
            {
                case 1: // UP - LEFT
                    position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 2: // UP - RIGHT
                    position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 3: // DOWN - LEFT
                    position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case 4: // DOWN - RIGHT
                    position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }
        private void SmokeUpdate(GameTime gameTime)
        {
            if (currentSmokeSpawnTime < smokeSpawnTime)
            {
                currentSmokeSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                EntityManager.InstantiateEntity<MetalThornExplosionSmoke>(_graphics, _content, position);

                currentSmokeSpawnTime = 0;
            }
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
