using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy
    {
        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public void MoveEnemy(Enemy enemy)
        {
            float x0 = enemy.X0;
            float y0 = enemy.Y0;
            float s = -enemy.Speed;
            float a = 0.05f;
            float p = 0.045f;
            float yi = enemy.Shape.Position.Y + s;
            float xi = x0 + a * System.MathF.Sin((2f * System.MathF.PI * (y0 - yi))/p);
            enemy.Shape.SetPosition(new Vec2F(xi, yi));
        }
    }
}