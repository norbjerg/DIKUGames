using DIKUArcade.Entities;
using DIKUArcade.Graphics;
namespace Galaga;
public class Enemy : Entity {

    private int hitpoints;
    private float speed;
    private IBaseImage enemyStrideRed;

    public Enemy(DynamicShape shape,
        IBaseImage image,
        IBaseImage enemyStrideRed)
    : base(shape, image) {
        hitpoints = 4;
        this.enemyStrideRed = enemyStrideRed;
        speed = 0.01f;
    }

    /// <summary>
    /// Method for enemy getting shot, returns true, if enemy dies.
    /// Checks if the enemy should get enraged and turn red.
    /// </summary>
    public bool getShot() {
        hitpoints -= 1;
        if (hitpoints < 0) {
            return true;
        }
        // Becomes enraged
        else if (hitpoints == 2) {
            hitpoints -= 1;
            this.Image = this.enemyStrideRed;
            speed *= 2;
            return false;
        }
        return false;
    }
}
