using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SlimeLab
{
    public static class WorldManager
    {
        private static GraphicsDeviceManager _graphics;
        private static ContentManager _content;

        private static Random _random;

        public static void Startup(GraphicsDeviceManager graphics, ContentManager content)
        {
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
            EntityManager.InstantiateEntity<PlayerEntity>(_graphics, _content);
        }
        private static void WorldTicked()
        {
            if (GameManager.IsGameEnded)
                return;

            EntityManager.InstantiateEntity<MoonCellEntity>(_graphics, _content);

            if(_random.Next(0, 100) < 50)
            {
                EntityManager.InstantiateEntity<MetalThornEntity>(_graphics, _content);
            }
        }
    }
}
