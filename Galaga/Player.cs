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
            // TODO: Finish this, so it uses the right max and min for both X and Y
            if (shape.Position.X >= 0 && shape.Position.X <= 100) {
                shape.Move();
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