using SlimeLab.Entities.Cells;
using SlimeLab.Entities.MetalThorn;
using SlimeLab.Entities.Player;

namespace SlimeLab.Managers
{
    public static class WorldManager
    {
        private static Core _core;

        public static void Startup(Core core)
        {
            _core = core;

            InstantiatePlayer();

            TickManager.OnTicked += WorldTicked;
        }
        public static void Cancel()
        {
            TickManager.OnTicked -= WorldTicked;
        }
        private static void InstantiatePlayer()
        {
            _ = EntityManager.InstantiateEntity<PlayerEntity>(_core);
        }
        private static void WorldTicked()
        {
            if (GameManager.IsGameEnded)
            {
                return;
            }

            _ = EntityManager.InstantiateEntity<MoonCellEntity>(_core);

            if (_core.Random.Next(0, 100) < 50)
            {
                _ = EntityManager.InstantiateEntity<MetalThornEntity>(_core);
            }
        }
    }
}
