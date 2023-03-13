using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga.Squadron;

class StandardFormation : ISquadron
{
    public EntityContainer<Enemy> Enemies { get; }

    public int MaxEnemies { get; }
    private float speed;

    public StandardFormation(float speed) {
        MaxEnemies = 8;
        Enemies = new EntityContainer<Enemy>(MaxEnemies);
        this.speed = speed;
    }

    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride)
    {
        for (int i = 0; i < MaxEnemies; i++) {
            Enemies.AddEntity(new Enemy(
                new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, enemyStride),
                new ImageStride(80, alternativeEnemyStride),
                speed));
        }
    }
}