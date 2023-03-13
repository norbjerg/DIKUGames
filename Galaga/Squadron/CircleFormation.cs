using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga.Squadron;

class CircleFormation : ISquadron
{
    public EntityContainer<Enemy> Enemies { get; }

    public int MaxEnemies { get; }

    public CircleFormation() {
        MaxEnemies = 8;
        Enemies = new EntityContainer<Enemy>(MaxEnemies);
    }

    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride)
    {
        float radius = 0.15f;
        for (int i = 0; i < MaxEnemies; i++) {
            Enemies.AddEntity(new Enemy(
                new DynamicShape(
                    new Vec2F(
                        0.45f + radius * System.MathF.Cos(
                            i * 2f * (float) System.Math.PI / MaxEnemies),
                        0.7f + radius * System.MathF.Sin(
                            i * 2f * (float) System.Math.PI / MaxEnemies)),
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, enemyStride),
                new ImageStride(80, alternativeEnemyStride))); 
        }
    }
}