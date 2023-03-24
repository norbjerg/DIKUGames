using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Physics;
using Galaga.MovementStrategy;
using Galaga.Squadron;

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        // private Entity backGroundImage; //Jeg havde jo ligesom lidt lavet min egen, så jeg ved ikke helt hvad vi skal gøre med denne? Slette? ;)))
        private Player player;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private EntityContainer<Enemy> enemies;
        private const int NUM_ENEMIES = 8;
        private IMovementStrategy movementStrategy;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;
        private AnimationContainer enemyExplosions;
        private List<Image> enemyStridesGreen;
        private List<Image> enemyStridesRed;
        private Background background;
        private Score scoreText;
        private int level;
        private Text levelCounter;
        private bool gameOver;
        private Text gameOverText;
        private Random rand = new Random();

        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning();
                GameRunning.instance.InitializeGameState();
            }
            return GameRunning.instance;
        }

        public void InitializeGameState() {
            // Assests
            playerShotImage = new Image(Path.Combine("..", "Galaga", "Assets", "Images", "BulletRed2.png"));
            List<Image> images = ImageStride.CreateStrides
                (4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
            enemies = new EntityContainer<Enemy>(NUM_ENEMIES);
            enemyStridesGreen = ImageStride.CreateStrides(
                2, Path.Combine("..", "Galaga", "Assets", "Images", "GreenMonster.png"));
            enemyStridesRed = ImageStride.CreateStrides(
                2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
            enemyExplosions = new AnimationContainer(NUM_ENEMIES);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("..", "Galaga", "Assets", "Images", "Explosion.png"));

            background = new Background(
                new Image(Path.Combine("..", "Galaga", "Assets", "Images", "SpaceBackground.png")),
                new DynamicShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)));

            scoreText = new Score(new Vec2F(0.05f, -0.2f), new Vec2F(0.25f, 0.25f));

            // Entities
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("..", "Galaga", "Assets", "Images", "Player.png")),
                GalagaBus.GetBus());
            playerShots = new EntityContainer<PlayerShot>();

            for (int i = 0; i < NUM_ENEMIES; i++) {
                enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStridesGreen),
                    new ImageStride(80, enemyStridesRed)));
            }
            movementStrategy = new NoMove();

            gameOver = false;
            gameOverText = new Text("GAME OVER", new Vec2F(0.1f,0f), new Vec2F(1f,0.7f));
            gameOverText.SetColor(new Vec3I(255,255,255));

            level = 1;
            levelCounter = new Text(
                "Level " + level, new Vec2F(0.5f, -0.2f), new Vec2F(0.25f, 0.25f));
            levelCounter.SetColor(new Vec3I(255,255,255));
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) KeyPress(key);
            else KeyRelease(key);
        }

        private void KeyPress(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Escape:
                    GalagaBus.GetBus().RegisterEvent(
                            new GameEvent{
                                EventType = GameEventType.WindowEvent,
                                Message = "QUIT_GAME",
                            }
                        );
                    break;
                case KeyboardKey.P:
                    GalagaBus.GetBus().RegisterEvent(
                        new GameEvent{
                            EventType = GameEventType.GameStateEvent,
                            Message = "CHANGE_STATE",
                            StringArg1 = "GAME_PAUSED"
                        }
                    );
                    break;
                case KeyboardKey.Left:
                    this.eventToPlayer("LEFT PRESS");
                    break;
                case KeyboardKey.Right:
                    this.eventToPlayer("RIGHT PRESS");
                    break;
                case KeyboardKey.Up:
                    this.eventToPlayer("UP PRESS");
                    break;
                case KeyboardKey.Down:
                    this.eventToPlayer("DOWN PRESS");
                    break;
            }
        }

        private void KeyRelease(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Left:
                    this.eventToPlayer("LEFT RELEASE");
                    break;
                case KeyboardKey.Right:
                    this.eventToPlayer("RIGHT RELEASE");
                    break;
                case KeyboardKey.Up:
                    this.eventToPlayer("UP RELEASE");
                    break;
                case KeyboardKey.Down:
                    this.eventToPlayer("DOWN RELEASE");
                    break;
                case KeyboardKey.Space:
                    playerShots.AddEntity(new PlayerShot(player.GetPosition(), playerShotImage));
                    break;
            }
        }

        private void eventToPlayer(string message) {
            GalagaBus.GetBus().RegisterEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        Message = message
                    }
                );
        }

        public void RenderState() {
            background.Render();
            if (!gameOver) {
                player.Render();
                enemies.RenderEntities();
                playerShots.RenderEntities();
                enemyExplosions.RenderAnimations();
                player.RenderHealth();
            }
            else
                gameOverText.RenderText();
            scoreText.RenderScore();
            levelCounter.RenderText();
        }

        public void ResetState() {
            this.InitializeGameState();
        }


        public void UpdateState() {
            if (!gameOver) {
                background.Update();
                player.Move();
                player.UpdateHealthBuffer();
                movementStrategy.MoveEnemies(enemies);
                IterateShots();
                IterateEnemies();
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
                                scoreText.IncrementScore(1);
                            }
                        }
                    });
                }
            });
        }

        private void IterateEnemies() {
            enemies.Iterate(enemy => {
                Vec2F playerPosition = player.GetPosition();
                Vec2F playerExtent = player.GetExtent();
                if (player.CollidedWithPlayer(enemy.Shape, true) || enemy.Shape.Position.Y < 0.05f)
                    gameOver = true;
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
                scoreText.IncrementScore(level);
                level++;
                levelCounter.SetText("Level " + level);
                ISquadron formation = nextFormation(rand.Next(1, 5));
                enemies = formation.Enemies;
                formation.CreateEnemies(enemyStridesGreen, enemyStridesRed);
                enemies.Iterate(enemy => {
                    enemy.IncreaseSpeed((0.0003f*level));
                });
                nextMovement(rand.Next(1, 3));
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
                    if (level > 4)
                        return new HellFormation();
                    else
                        return new StandardFormation();
                default:
                    return new StandardFormation();
            }
        }

        private void nextMovement(int num) {
            switch (num) {
                case 1:
                    movementStrategy = new Down();
                    break;
                case 2:
                    movementStrategy = new ZigZagDown();
                    break;
            }
        }
    }
}
