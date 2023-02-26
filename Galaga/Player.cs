using DIKUArcade.Entities;
using DIKUArcade.Graphics;
namespace Galaga {
public class Player {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft;
        private float moveRight;
        private const float MOVEMENT_SPEED = 0.01f;

        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
            moveLeft = 0.0f;
            moveRight = 0.0f;
        }

        public void Render() {
            entity.RenderEntity();
        }

        public void Move() {
            float min = 0.0f;
            // NOTE: Seems like the shape has position in its left corner, so 0.9 works best
            float max = 0.9f;
            if (shape.Position.X > min && shape.Position.X < max) {
                shape.Move();
            }
            else {
                if (shape.Position.X <= min) {
                    shape.MoveX(0.01f);
                }
                else if (shape.Position.X >= max) {
                    shape.MoveX(-0.01f);
                }
            }
            // TODO: move the shape and guard against the window borders
        }

        public void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0;
            }
            UpdateDirection();
            // TODO:set moveLeft appropriately and call UpdateDirection()
        }
        public void SetMoveRight(bool val) {
            if (val) {
                moveLeft = MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0;
            }
            UpdateDirection();
            // TODO:set moveRight appropriately and call UpdateDirection()
        }

        private void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
        }
    }
}