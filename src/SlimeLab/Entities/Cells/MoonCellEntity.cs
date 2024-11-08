using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SlimeLab.Entities.Particles;
using SlimeLab.Entities.Player;
using SlimeLab.Entities.Projectiles;

namespace SlimeLab.Entities.Cells
{
    public class MoonCellEntity : Entity
    {
        // ENTITIES
        private PlayerEntity playerEntity;

        // CELL STATUS
        private readonly float cellPower = 0.2f;

        // CELL TEXTURE ANIMATIONS
        private int currentState;
        private readonly float changeStateTime = 0.1f;
        private float changeStateCurrentTime = 0f;

        private bool collected;

        public MoonCellEntity(Core core) : base(core)
        {

        }

        public override void Initialize()
        {
            this.Position = new(this.Core.Random.Next(32, this.Core.GraphicsDeviceManager.PreferredBackBufferWidth - 32), this.Core.Random.Next(32, this.Core.GraphicsDeviceManager.PreferredBackBufferHeight - 32));

            this.playerEntity = this.Core.EntityManager.GetEntity<PlayerEntity>();
        }

        public override void Update(GameTime gameTime)
        {
            float distance = Vector2.Distance(this.Position, this.playerEntity.Position);
            if (distance < this.playerEntity.PlayerRadius && !this.collected)
            {
                this.Core.ScoreManager.Score++;
                this.collected = true;

                this.playerEntity.NextPlayerScale += new Vector2(this.cellPower, this.cellPower);

                _ = this.Core.EntityManager.InstantiateEntity<MoonCellDestructionParticle>(this.Position);
                _ = this.Core.EntityManager.InstantiateEntity<MoonCellProjectile>(this.Position);

                this.Core.CollectSoundEffect.CreateInstance().Play();
                this.Core.EntityManager.DestroyEntity(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationUpdate(gameTime);
            spriteBatch.Draw(this.Core.CellSheetTextures[this.currentState], this.Position, null, Color.White, 0f, new Vector2(this.Core.CellSheetTextures[this.currentState].Width / 2, this.Core.CellSheetTextures[this.currentState].Height / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void AnimationUpdate(GameTime gameTime)
        {
            if (this.changeStateCurrentTime < this.changeStateTime)
            {
                this.changeStateCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (this.currentState < this.Core.CellSheetTextures.Length - 1)
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
