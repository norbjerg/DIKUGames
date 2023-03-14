using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga;
public class Health {
    private int health;
    private Text display;
    public Health (Vec2F position, Vec2F extent) {
        health = 3;
        display = new Text("Health: " + health.ToString(), position, extent);
        display.SetColor(new Vec3I(255,255,255));
    }

    /// <summary>
    /// The health is decreased, by decreasing the health by 1.
    /// If the health becomes zero nothing happens when this method is called.
    /// The display text is also updated.
    /// </summary>
    public void LoseHealth () {
        if (health > 0) {
            health -= 1;
            display.SetText("Health: " + health.ToString());
        }
    }
    public void RenderHealth () {
        display.RenderText();
    }

    public int GetHealth() {
        return health;
    }

}