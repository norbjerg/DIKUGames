using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga;

class Background {
    private Image image;
    private DynamicShape shape;
    private DynamicShape shapeCopy;
    //Backgorund rolls as paper, so the background needs to be 2x
    private static float SPEED = 0.01F;

    public Background(Image image, DynamicShape shape){
        this.image= image;
        this.shape = shape;
        this.shapeCopy = new DynamicShape(new Vec2F(0f, 1f), new Vec2F(1f, 1f));
    }

    public void Render(){
        image.Render(shapeCopy);
        image.Render(shape);
    }

    private void roll(DynamicShape rollShape){
        rollShape.MoveY(-SPEED);
        if (rollShape.Position.Y <= -1f)
            rollShape.SetPosition(new Vec2F(0f, 1f -SPEED));
    }

    public void Update(){
        roll(shape);
        roll(shapeCopy);
    }
}
