using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga.Squadron;

class DiagonalFormation : ISquadron
{
    public EntityContainer<Enemy> Enemies { get; }

    public int MaxEnemies { get; }

    public DiagonalFormation() {
        MaxEnemies = 7;
        Enemies = new EntityContainer<Enemy>(MaxEnemies);
    }

    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride)
    {
        for (int i = 0; i < MaxEnemies; i++) {
            Enemies.AddEntity(new Enemy(
                new DynamicShape(
                    new Vec2F(0.2f + (float)i * 0.1f, 0.3f + (float)i * 0.1f),
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, enemyStride),
                new ImageStride(80, alternativeEnemyStride)));
        }
    }
}
