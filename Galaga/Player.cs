using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
public class Player {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft;
        private float moveRight;
		private float moveUp;
		private float moveDown;
        private const float MOVEMENT_SPEED = 0.01f;

        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
            moveLeft = 0.0f;
            moveRight = 0.0f;
			moveUp = 0.0f;
			moveDown = 0.0f;
        }

        public void Render() {
            entity.RenderEntity();
        }

        public void Move() {
            UpdateDirection();

            //Normalises the vector, so that it doesen't move at 2x speed when travelling diagonal
            if (shape.Direction.X != 0 && shape.Direction.Y != 0){
                double length =
                    System.Math.Sqrt(System.Math.Pow(shape.Direction.X, 2d) +
                    System.Math.Pow(shape.Direction.Y, 2d));

                shape.Direction.X = shape.Direction.X / (float)length * MOVEMENT_SPEED;
                shape.Direction.Y = shape.Direction.Y / (float)length * MOVEMENT_SPEED;

            }
            float potX = GetPosition().X + shape.Direction.X;
            float potY = GetPosition().Y + shape.Direction.Y;
            

            float min = 0.0f+shape.Extent.X/2;
            // NOTE: Seems like the shape has position in its left corner, so 0.9 works best
            float max = 0.9f-shape.Extent.X/2;
			if (min < potX && potX < max) {
                if (min < potY && potY < max/2) {
					shape.Move();
                }
			}
		}

        public void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0;
            }
        }

        public void SetMoveRight(bool val) {
            if (val) {
                moveRight = MOVEMENT_SPEED;
            }
            else {
                moveRight = 0;
            }
        }

        public void SetMoveUp(bool val) {
            if (val) {
                moveUp = MOVEMENT_SPEED;
            }
            else {
                moveUp = 0;
            }
        }

        public void SetMoveDown(bool val) {
            if (val) {
                moveDown = -MOVEMENT_SPEED;
            }
            else {
                moveDown = 0;
            }
        }

        private void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
			shape.Direction.Y = moveUp + moveDown;
        }

        public Vec2F GetPosition() {
            // NOTE: Since the position is actually in the left corner, we offset it to be centered
            return new Vec2F(entity.Shape.Position.X + 0.05f, entity.Shape.Position.Y + 0.05f);
        }
    }
}
