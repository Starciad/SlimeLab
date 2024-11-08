using SlimeLab.Entities.Cells;
using SlimeLab.Entities.MetalThorn;
using SlimeLab.Entities.Player;
using SlimeLab.Objects;

namespace SlimeLab.Managers
{
    public sealed class WorldManager : GameObject
    {
        public WorldManager(Core core) : base(core)
        {

        }

        public override void Initialize()
        {
            InstantiatePlayer();

            this.Core.TickManager.OnTicked += WorldTicked;
        }

        public void Cancel()
        {
            this.Core.TickManager.OnTicked -= WorldTicked;
        }

        private void InstantiatePlayer()
        {
            _ = this.Core.EntityManager.InstantiateEntity<PlayerEntity>();
        }

        private void WorldTicked()
        {
            if (this.Core.GameManager.IsGameEnded)
            {
                return;
            }

            _ = this.Core.EntityManager.InstantiateEntity<MoonCellEntity>();

            if (this.Core.Random.Next(0, 100) < 50)
            {
                _ = this.Core.EntityManager.InstantiateEntity<MetalThornEntity>();
            }
        }
    }
}
