using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.Collections.Generic;

namespace Galaga {
public class Player : IGameEventProcessor {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft;
        private float moveRight;
		private float moveUp;
		private float moveDown;
        private const float MOVEMENT_SPEED = 0.01f;
        private GameEventBus eventBus;


        public Player(DynamicShape shape, IBaseImage image, GameEventBus eventBus) {
            entity = new Entity(shape, image);
            this.shape = shape;
            moveLeft = 0.0f;
            moveRight = 0.0f;
			moveUp = 0.0f;
			moveDown = 0.0f;

            this.eventBus = eventBus;
        }

        public void Render() {
            entity.RenderEntity();
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
                if (min < potY && potY < maxY/2) {
					shape.Move();
                }
			}
		}

        private void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0;
            }
        }

        private void SetMoveRight(bool val) {
            if (val) {
                moveRight = MOVEMENT_SPEED;
            }
            else {
                moveRight = 0;
            }
        }

        private void SetMoveUp(bool val) {
            if (val) {
                moveUp = MOVEMENT_SPEED;
            }
            else {
                moveUp = 0;
            }
        }

        private void SetMoveDown(bool val) {
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

        public void ProcessEvent(GameEvent gameEvent)
        {
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
    }
}
