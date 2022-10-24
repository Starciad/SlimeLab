using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlimeLab
{
    public class MetalThornEntity : Entity
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        private Random _random;

        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private float metalThornPower = 0.8f;

        // CELL WORLD MAP
        private float metalThornRadius = 32f;
        private float metalThornSpeed = 2f;
        private Vector2 metalThornPosition;
        private Vector2 metalThornScale;

        private Vector2 nextMetalThornPosition;

        // CELL TEXTURE ANIMATIONS
        private Texture2D[] metalThornSheetTextures;
        private int currentState;

        private float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        //=========================//

        protected override void OnInstantiate(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _content = content;

            _random = new();

            metalThornSheetTextures = new Texture2D[]
            {
                _content.Load<Texture2D>(@"Sprites\MetalThorn\MetalThorn1"),
                _content.Load<Texture2D>(@"Sprites\MetalThorn\MetalThorn2"),
            };

            metalThornScale = new(1, 1);
            metalThornPosition = new(_random.Next(0, _graphics.PreferredBackBufferWidth),
                                     _random.Next(0, _graphics.PreferredBackBufferHeight));

            //metalThornPosition = new(_random.Next(0, _graphics.PreferredBackBufferWidth),
            //         _random.Next(0, _graphics.PreferredBackBufferHeight));

            metalThornPosition.X = metalThornPosition.X < (_graphics.PreferredBackBufferWidth / 2) ? -100 : metalThornPosition.X = _graphics.PreferredBackBufferWidth + 100;
            metalThornPosition.Y = metalThornPosition.Y < (_graphics.PreferredBackBufferHeight / 2) ? metalThornPosition.X = _graphics.PreferredBackBufferWidth + 100 : -100;

            nextMetalThornPosition = metalThornPosition;
        }

        //=========================//

        protected override void OnStartup()
        {
            playerEntity = EntityManager.GetEntity<PlayerEntity>();
        }

        //=========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            MoveToPlayerUpdate(gameTime);
            CollisionWithPlayerUpdate();
            MetalThornPositionUpdate(gameTime);
        }

        private void MoveToPlayerUpdate(GameTime gameTime)
        {
            if(playerEntity.PlayerPosition.X + _random.Next(-1000, 1000) < metalThornPosition.X)
            {
                nextMetalThornPosition.X -= _random.Next(1, 20) + metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                nextMetalThornPosition.X += _random.Next(1, 20) + metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (playerEntity.PlayerPosition.Y + _random.Next(-1000, 1000) < metalThornPosition.Y)
            {
                nextMetalThornPosition.Y -= _random.Next(1, 20) + metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                nextMetalThornPosition.Y += _random.Next(1, 20) + metalThornSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void CollisionWithPlayerUpdate()
        {
            float distance = Vector2.Distance(metalThornPosition, playerEntity.PlayerPosition);
            if (distance < playerEntity.PlayerRadius && !collected)
            {
                collected = true;
                playerEntity.NextPlayerScale -= new Vector2(metalThornPower, metalThornPower);

                for (int i = 0; i < 4; i++)
                {
                    MetalThornExplosion explosion = EntityManager.InstantiateEntity<MetalThornExplosion>(_graphics, _content, metalThornPosition);
                    explosion.Direction = i + 1;
                }

                EntityManager.DestroyEntity(this);
            }
        }
        private void MetalThornPositionUpdate(GameTime gameTime)
        {
            metalThornPosition = Vector2.Lerp(metalThornPosition, nextMetalThornPosition, 6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(metalThornSheetTextures[currentState],
                             metalThornPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(metalThornSheetTextures[currentState].Width / 2, metalThornSheetTextures[currentState].Height / 2),
                             metalThornScale,
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
                if (currentState < metalThornSheetTextures.Length - 1)
                    currentState++;
                else
                    currentState = 0;

                changeStateCurrentTime = 0;
            }
        }
    }
}
