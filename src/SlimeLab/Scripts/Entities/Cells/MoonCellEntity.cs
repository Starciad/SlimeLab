using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SlimeLab
{
    public class MoonCellEntity : Entity
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        private Random _random;

        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private readonly float cellPower = 0.2f;

        // CELL WORLD MAP
        private Vector2 cellPosition;
        private Vector2 cellScale;

        // CELL TEXTURE ANIMATIONS
        private Texture2D[] cellSheetTextures;
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        // SOUNDS
        private Song collectSoundEffect;

        //=========================//

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _content = content;

            _random = new();

            cellSheetTextures = new Texture2D[]
            {
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell1"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell2"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell3"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell4"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell5"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell6"),
                _content.Load<Texture2D>(@"Sprites\Cells\MoonCell\MoonCell7"),
            };

            collectSoundEffect = _content.Load<Song>(@"Sounds\coin");

            cellScale = new(1, 1);
            cellPosition = new(_random.Next(32, _graphics.PreferredBackBufferWidth - 32),
                                 _random.Next(32, _graphics.PreferredBackBufferHeight - 32));
        }

        //=========================//

        protected override void OnStartup()
        {
            playerEntity = EntityManager.GetEntity<PlayerEntity>();
        }

        //=========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            float distance = Vector2.Distance(cellPosition, playerEntity.PlayerPosition);
            if (distance < playerEntity.PlayerRadius && !collected)
            {
                ScoreManager.Score++;
                collected = true;

                playerEntity.NextPlayerScale += new Vector2(cellPower, cellPower);
                EntityManager.InstantiateEntity<MoonCellDestructionParticle>(_graphics, _content, cellPosition);

                MediaPlayer.Play(collectSoundEffect);
                EntityManager.DestroyEntity(this);
            }
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(cellSheetTextures[currentState],
                             cellPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(cellSheetTextures[currentState].Width / 2, cellSheetTextures[currentState].Height / 2),
                             cellScale,
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
                if (currentState < cellSheetTextures.Length - 1)
                    currentState++;
                else
                    currentState = 0;

                changeStateCurrentTime = 0;
            }
        }
    }
}
