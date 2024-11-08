using Microsoft.Xna.Framework;

using SlimeLab.Objects;

namespace SlimeLab.Entities
{
    public abstract class Entity : GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public Entity(Core core) : base(core)
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
        }

        public virtual void Destroy() { return; }
    }
}
