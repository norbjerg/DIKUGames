using DIKUArcade.Entities;
using DIKUArcade.Graphics;
namespace Galaga;
public class Enemy : Entity {

    public int hitpoints {get; private set; }
    public float Speed { get; private set; }
    private IBaseImage enemyStrideRed;
    public float X0 { get; }
    public float Y0 { get; }

    public Enemy(DynamicShape shape,
        IBaseImage image,
        IBaseImage enemyStrideRed,
        float speed = 0.0003f)
    : base(shape, image) {
        hitpoints = 4;
        this.enemyStrideRed = enemyStrideRed;
        Speed = speed;
        X0 = shape.Position.X;
        Y0 = shape.Position.Y;
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
			IncreaseSpeed(Speed * 2);
            return false;
        }
        return false;
    }

	public void IncreaseSpeed(float spdincrement) {
		Speed += spdincrement;
	}
}
