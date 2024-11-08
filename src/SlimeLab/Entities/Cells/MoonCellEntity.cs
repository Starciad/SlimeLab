using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.Particles;
using SlimeLab.Entities.Player;
using SlimeLab.Managers;

namespace SlimeLab.Entities.Cells
{
    public class MoonCellEntity : Entity
    {
        private Core _core;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;

        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private readonly float cellPower = 0.2f;

        // CELL WORLD MAP
        private Vector2 cellPosition;
        private Vector2 cellScale;

        // CELL TEXTURE ANIMATIONS
        private int currentState;

        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        //=========================//

        protected override void OnInstantiate(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            this._core = core;
            this._graphics = graphics;
            this._content = content;

            this.cellScale = new(1, 1);
            this.cellPosition = new(this._core.Random.Next(32, this._graphics.PreferredBackBufferWidth - 32),
                                 this._core.Random.Next(32, this._graphics.PreferredBackBufferHeight - 32));
        }

        //=========================//

        protected override void OnStartup()
        {
            this.playerEntity = EntityManager.GetEntity<PlayerEntity>();
        }

        //=========================//

        protected override void OnUpdate(GameTime gameTime)
        {
            float distance = Vector2.Distance(this.cellPosition, this.playerEntity.PlayerPosition);
            if (distance < this.playerEntity.PlayerRadius && !this.collected)
            {
                ScoreManager.Score++;
                this.collected = true;

                this.playerEntity.NextPlayerScale += new Vector2(this.cellPower, this.cellPower);
                _ = EntityManager.InstantiateEntity<MoonCellDestructionParticle>(this._core, this._graphics, this._content, this.cellPosition);

                this._core.CollectSoundEffect.CreateInstance().Play();
                EntityManager.DestroyEntity(this);
            }
        }

        //=========================//

        protected override void OnRender(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this._core.CellSheetTextures[this.currentState],
                             this.cellPosition,
                             null,
                             Color.White,
                             0f,
                             new Vector2(this._core.CellSheetTextures[this.currentState].Width / 2, this._core.CellSheetTextures[this.currentState].Height / 2),
                             this.cellScale,
                             SpriteEffects.None,
                             0f);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.changeStateCurrentTime < this.changeStateTime)
            {
                this.changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this._core.CellSheetTextures.Length - 1)
                {
                    this.currentState++;
                }
                else
                {
                    this.currentState = 0;
                }

                this.changeStateCurrentTime = 0;
            }
        }
    }
}
