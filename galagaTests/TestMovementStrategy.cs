using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.GUI;
using Galaga.MovementStrategy;
using Galaga.GalagaStates;
using Galaga;

namespace GalagaTests {
    [TestFixture]
    public class MovementStrategyTesting {

		private GameEventBus? eventBus;
		private StateMachine? stateMachine;
		private Player? testPlayer;
		private List<Image>? enemyStridesGreen;
		private List<Image>? enemyStridesRed;
		private IMovementStrategy? testStrategy;
		private Enemy? testEnemy;

		[SetUp]
		public void Init() {
			Window.CreateOpenGLContext();
			eventBus = new GameEventBus();

			eventBus.InitializeEventBus(new List<GameEventType> {
				GameEventType.GameStateEvent,
				GameEventType.InputEvent,
				GameEventType.WindowEvent });

			stateMachine = new StateMachine();

			// ".." to get the right directory
            testPlayer = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("..", "Galaga", "Assets", "Images", "Player.png")),
                GalagaBus.GetBus());
			eventBus.Subscribe(GameEventType.WindowEvent, testPlayer);

            enemyStridesGreen = ImageStride.CreateStrides(
                2, Path.Combine("..", "Galaga", "Assets", "Images", "GreenMonster.png"));
            enemyStridesRed = ImageStride.CreateStrides(
                2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
            testEnemy = new Enemy(
                    new DynamicShape(
                        new Vec2F(0.2f , 0.3f),
                        new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStridesGreen),
                    new ImageStride(80, enemyStridesRed));
			testStrategy = new Down();
		}

		[Test]
		public void TestStrategyMoveEnemy() {
			if (testStrategy is null || testEnemy is null) {
				Assert.Fail();
				return;
			}
			float startPos = testEnemy.Shape.Position.Y;
			testStrategy.MoveEnemy(testEnemy);
			Assert.AreEqual(startPos-testEnemy.Speed, testEnemy.Shape.Position.Y);
		}
	}
}
