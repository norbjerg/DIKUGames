using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    class PlayerShot : Entity {
        private static Vec2F extent = new Vec2F(0.008f, 0.021f);
        private static Vec2F direction = new Vec2F(0.0f, 0.1f);
        
        public Vec2F Direction {
            get { return extent; }
            private set { extent = value; }
        }
        public PlayerShot(Vec2F position, IBaseImage image)
        : base(new DynamicShape(position.X, position.Y, extent.X, extent.Y), image) {}
    }
}