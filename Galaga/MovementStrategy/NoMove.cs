using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class NoMove : IMovementStrategy
    {
        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            return;
        }

        public void MoveEnemy(Enemy enemy)
        {
            return;
        }
    }
}