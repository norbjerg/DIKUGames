using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy
    {
        public void MoveEnemies(EntityContainer<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public void MoveEnemy(Enemy enemy)
        {
            enemy.Shape.MoveY(-enemy.Speed);
        }
    }
}