using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga;
public class Health {
    public int health {get; private set;}
    public int loseHealthBuffer {get; private set;}
    private Text display;
    public Health (Vec2F position, Vec2F extent) {
        health = 3;
        loseHealthBuffer = 0;
        display = new Text("Health: " + health.ToString(), position, extent);
        display.SetColor(new Vec3I(255,255,255));
    }

    /// <summary>
    /// The health is decreased, by decreasing the health by 1.
    /// If the health becomes zero nothing happens when this method is called.
    /// The display text is also updated.
    /// </summary>
    /// <returns>
    /// Returns true if health is equal to zero
    /// </returns>

    public bool LoseHealth () {
        if (health > 0 && loseHealthBuffer == 0) {
            health--;
            loseHealthBuffer = 60;
            display.SetText("Health: " + health.ToString());
        }
        return health == 0;
    }
    public void RenderHealth () {
        display.RenderText();
    }

    public void UpdateHealthBuffer() {
        if (loseHealthBuffer > 0) loseHealthBuffer--;
    }
}
