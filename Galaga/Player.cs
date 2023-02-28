using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

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
			if (shape.Direction.X > 0) {
				if (shape.Position.X < max) {
					shape.Move();
				}
			}
			if (shape.Direction.X < 0) {
				if (shape.Position.X > min) {
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
            UpdateDirection();
        }
        public void SetMoveRight(bool val) {
            if (val) {
                moveLeft = MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0;
            }
            UpdateDirection();
        }

        private void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
        }

        public Vec2F GetPosition() {
            // NOTE: Since the position is actually in the left corner, we offset it to be centered
            return new Vec2F(entity.Shape.Position.X + 0.05f, entity.Shape.Position.Y + 0.05f);
        }
    }
}
