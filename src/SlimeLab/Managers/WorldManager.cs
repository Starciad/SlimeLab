using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using SlimeLab.Entities.Cells;
using SlimeLab.Entities.MetalThorn;
using SlimeLab.Entities.Player;

using System;

namespace SlimeLab.Managers
{
    public static class WorldManager
    {
        private static Core _core;
        private static GraphicsDeviceManager _graphics;
        private static ContentManager _content;

        private static Random _random;

        public static void Startup(Core core, GraphicsDeviceManager graphics, ContentManager content)
        {
            _core = core;
            _graphics = graphics;
            _content = content;

            _random = new();

            InstantiatePlayer();

            TickManager.OnTicked += WorldTicked;
        }
        public static void Cancel()
        {
            TickManager.OnTicked -= WorldTicked;
        }
        private static void InstantiatePlayer()
        {
            _ = EntityManager.InstantiateEntity<PlayerEntity>(_core, _graphics, _content);
        }
        private static void WorldTicked()
        {
            if (GameManager.IsGameEnded)
            {
                return;
            }

            _ = EntityManager.InstantiateEntity<MoonCellEntity>(_core, _graphics, _content);

            if (_random.Next(0, 100) < 50)
            {
                _ = EntityManager.InstantiateEntity<MetalThornEntity>(_core, _graphics, _content);
            }
        }
    }
}
