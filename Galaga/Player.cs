using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Physics;

namespace Galaga {
public class Player : IGameEventProcessor {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft;
        private float moveRight;
		private float moveUp;
		private float moveDown;
        private Health health;
        private const float MOVEMENT_SPEED = 0.01f;
        private GameEventBus eventBus;


        public Player(DynamicShape shape, IBaseImage image, GameEventBus eventBus) {
            entity = new Entity(shape, image);
            this.shape = shape;
            moveLeft = 0.0f;
            moveRight = 0.0f;
			moveUp = 0.0f;
			moveDown = 0.0f;
            health = new Health(new Vec2F(0.85f, -0.2f), new Vec2F(0.25f, 0.25f));
            
            this.eventBus = eventBus;
        }

        public void Render() {
            //Blinking when taking damage!
            if (this.health.loseHealthBuffer / 10 % 2 == 0) 
                entity.RenderEntity();
        }

        public void RenderHealth() {
            this.health.RenderHealth();
        }

        public void UpdateHealthBuffer() {
            this.health.UpdateHealthBuffer();
        }

        public void Move() {
            UpdateDirection();
            //Normalizes the vector, so that it doesn't move at 2x speed when traveling diagonal
            if (shape.Direction.X != 0 && shape.Direction.Y != 0){
                double length =
                    System.Math.Sqrt(System.Math.Pow(shape.Direction.X, 2d) +
                    System.Math.Pow(shape.Direction.Y, 2d));

                shape.Direction.X = shape.Direction.X / (float)length * MOVEMENT_SPEED;
                shape.Direction.Y = shape.Direction.Y / (float)length * MOVEMENT_SPEED;

            }
            float potX = GetPosition().X + shape.Direction.X;
            float potY = GetPosition().Y + shape.Direction.Y;
            

            float min = 0.0f + shape.Extent.X/2;
            // NOTE: Seems like the shape has position in its left corner, so 0.9 works best
            float maxY = 0.9f - shape.Extent.X/2;
            float maxX = 0.9f + shape.Extent.X/2;

			if (min < potX && potX < maxX) {
                if (min < potY && potY < maxY) {
					shape.Move();
                }
			}
		}

        private void SetMoveLeft(bool val) {
            moveLeft = (val) ? -MOVEMENT_SPEED : 0;
        }

        private void SetMoveRight(bool val) {
            moveRight = (val) ? MOVEMENT_SPEED : 0;
        }

        private void SetMoveUp(bool val) {
            moveUp = (val) ? MOVEMENT_SPEED : 0;
        }

        private void SetMoveDown(bool val) {
            moveDown = (val) ? -MOVEMENT_SPEED : 0;
        }

        private void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
			shape.Direction.Y = moveUp + moveDown;
        }

        public Vec2F GetPosition() {
            // NOTE: Since the position is actually in the left corner, we offset it to be centered
            return new Vec2F(entity.Shape.Position.X + 0.05f, entity.Shape.Position.Y + 0.05f);
        }

        public Vec2F GetExtent() => entity.Shape.Extent;

        public void ProcessEvent(GameEvent gameEvent) {

            if (gameEvent.EventType == GameEventType.InputEvent) {
                switch (gameEvent.Message) {
                    case ("UP PRESS"):
                        SetMoveUp(true);
                        break;
                    case ("DOWN PRESS"):
                        SetMoveDown(true);
                        break;
                    case ("LEFT PRESS"):
                        SetMoveLeft(true);
                        break;
                    case ("RIGHT PRESS"):
                        SetMoveRight(true);
                        break;
                    case ("UP RELEASE"):
                        SetMoveUp(false);
                        break;
                    case ("DOWN RELEASE"):
                        SetMoveDown(false);
                        break;
                    case ("LEFT RELEASE"):
                        SetMoveLeft(false);
                        break;
                    case ("RIGHT RELEASE"):
                        SetMoveRight(false);
                        break;
                }
            }
        }

        public bool CollidedWithPlayer(Shape otherShape, bool isEvil)  {
            CollisionData collision = CollisionDetection.Aabb(this.shape, otherShape);
            if (collision.Collision && isEvil)
                return this.health.LoseHealth();
            return false;
        }
    }
}
