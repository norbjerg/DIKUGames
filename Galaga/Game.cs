using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using System.Collections.Generic;
using Galaga.Squadron;

namespace Galaga
{
    public class Game : DIKUGame, IGameEventProcessor
    {
        private Player player;
        private GameEventBus eventBus;
        private EntityContainer<Enemy> enemies;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private Score scoreText;
        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;
        private List<Image> enemyStridesGreen;
        private List<Image> enemyStridesRed;
        private MovementStrategy.IMovementStrategy movementStrategy;
        private int level;
        private Health health;
        private Text levelCounter;
        private bool gameOver;
		private Text gameOverText;
        private int loseHealthBuffer;

        public Game(WindowArgs windowArgs) : base(windowArgs) {
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, GameEventType.WindowEvent });
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")),
                eventBus);
            window.SetKeyEventHandler(KeyHandler);
            List<Image> images = ImageStride.CreateStrides
                (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            const int numEnemies = 8;
            enemies = new EntityContainer<Enemy>(numEnemies);
            enemyStridesGreen = ImageStride.CreateStrides(
                2, Path.Combine("Assets", "Images", "GreenMonster.png"));
            enemyStridesRed = ImageStride.CreateStrides(
                2, Path.Combine("Assets", "Images", "RedMonster.png"));
            for (int i = 0; i < numEnemies; i++) {
                enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStridesGreen),
                    new ImageStride(80, enemyStridesRed)));
            }
            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

            scoreText = new Score(new Vec2F(0.05f, -0.2f), new Vec2F(0.25f, 0.25f));
            enemyExplosions = new AnimationContainer(numEnemies);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            movementStrategy = new MovementStrategy.ZigZagDown();
            level = 1;
            health = new Health(new Vec2F(0.85f, -0.2f), new Vec2F(0.25f, 0.25f));
            levelCounter = new Text(
                "Level " + level, new Vec2F(0.5f, -0.2f), new Vec2F(0.25f, 0.25f));
            levelCounter.SetColor(new Vec3I(255,255,255));
            gameOver = false;
			gameOverText = new Text("GAME OVER", new Vec2F(0.1f,0f), new Vec2F(1f,0.7f));
			gameOverText.SetColor(new Vec3I(255,255,255));
            loseHealthBuffer = 0;
        }

        public override void Render()
        {
            window.Clear();
            if (!gameOver) {
                scoreText.RenderScore();
                player.Render();
                enemies.RenderEntities();
                playerShots.RenderEntities();
                enemyExplosions.RenderAnimations();
                health.RenderHealth();
            }
            levelCounter.RenderText();
			if (gameOver) {
				gameOverText.RenderText();
			}
        }

        public override void Update()
        {
            window.PollEvents();
            eventBus.ProcessEventsSequentially();
            if (!gameOver) {
                player.Move();
                movementStrategy.MoveEnemies(enemies);
                IterateShots();
                IterateEnemies();
            }

        }

        private void KeyPress(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Escape:
                    GameEvent game_event = new GameEvent();
                    game_event.EventType = GameEventType.WindowEvent;
                    game_event.Message = "Close window";
                    eventBus.RegisterEvent(game_event);
                    break;
                case KeyboardKey.Left:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="LEFT PRESS"
                    });
                    break;
                case KeyboardKey.Right:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="RIGHT PRESS"
                    });
                    break;
                case KeyboardKey.Up:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="UP PRESS"
                    });
                    break;
                case KeyboardKey.Down:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="DOWN PRESS"
                    });
                    break;
            }
        }
        private void KeyRelease(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Left:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="LEFT RELEASE"
                    });
                    break;
                case KeyboardKey.Right:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="RIGHT RELEASE"
                    });
                    break;
                case KeyboardKey.Up:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="UP RELEASE"
                    });
                    break;
                case KeyboardKey.Down:
                    eventBus.RegisterEvent (new GameEvent {
                        EventType = GameEventType.InputEvent,
                        From=this,
                        To=player,
                        Message="DOWN RELEASE"
                    });
                    break;
                case KeyboardKey.Space:
                    playerShots.AddEntity(new PlayerShot(player.GetPosition(), playerShotImage));
                    break;
            }
        }
        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                KeyPress(key);
            }
            else {
                KeyRelease(key);
            }
        }
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.WindowEvent) {
                if (gameEvent.Message == "Close window") {
                    window.CloseWindow();
                }
            }
        }


        private void IterateShots() {
            playerShots.Iterate(shot => {
                shot.Shape.MoveY(shot.Shape.AsDynamicShape().Direction.Y);
                if (shot.Shape.Position.Y > 1) {
                    shot.DeleteEntity();
                } else {
                    enemies.Iterate(enemy => {
                        CollisionData collision = CollisionDetection.Aabb(
                            shot.Shape.AsDynamicShape(), enemy.Shape);

                        if (collision.Collision) {
                            shot.DeleteEntity();
                            if (enemy.getShot()) {
                                nextLevel();
                                enemy.DeleteEntity();
                                AddExplosion(enemy.Shape.Position, enemy.Shape.Extent);
                                scoreText.IncrementScore();
                            }
                        }
                    });
                }
            });
        }

        public void AddExplosion(Vec2F position, Vec2F extent) {
            enemyExplosions.AddAnimation(
                new StationaryShape(
                    position.X,
                    position.Y,
                    extent.X,
                    extent.Y),
                EXPLOSION_LENGTH_MS,
                new ImageStride(
                    (int) EXPLOSION_LENGTH_MS / 8,
                    explosionStrides));
        }

        private void nextLevel() {
            if (enemies.CountEntities() <= 1) {
				Random rnd = new Random();
				int formationNum = rnd.Next(1, 5);
                level += 1;
                levelCounter.SetText("Level " + level);
                ISquadron formation = nextFormation(formationNum);
                enemies = formation.Enemies;
                formation.CreateEnemies(enemyStridesGreen, enemyStridesRed);
				enemies.Iterate(enemy => {
					enemy.IncreaseSpeed((0.0003f*level));
				});
            }
        }

		private ISquadron nextFormation(int num){
			switch (num) {
				case 1:
					return new StandardFormation();
				case 2:
					return new DiagonalFormation();
				case 3:
					return new CircleFormation();
				case 4:
					if (level > 4) {
						return new HellFormation();
					} else {
						return new StandardFormation();
					}
				default:
					return new StandardFormation();
			}
		}


        private void IterateEnemies() {
            enemies.Iterate(enemy => {
                Vec2F playerPosition = player.GetPosition();
                Vec2F playerExtent = player.GetExtent();

                if ((enemy.Shape.Position.X < playerPosition.X + playerExtent.X/2
                    && enemy.Shape.Position.X > playerPosition.X - playerExtent.X/2)
                    && (enemy.Shape.Position.Y < playerPosition.Y + playerExtent.Y/2
                    && enemy.Shape.Position.Y > playerPosition.Y - playerExtent.Y/2)) {

                    if (loseHealthBuffer == 0) {
                        health.LoseHealth();
                        loseHealthBuffer = 10;
                    }
                    else {
                        loseHealthBuffer -= 1;
                    }
                    if (health.GetHealth() == 0) {
                        gameOver = true;
                    }
                }

                if (enemy.Shape.Position.Y < 0.05f) {
                    gameOver = true;
                }
            });
        }
    }
}
