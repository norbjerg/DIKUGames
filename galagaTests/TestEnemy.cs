using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.GUI;
using Galaga.GalagaStates;
using Galaga;

namespace GalagaTests {
    [TestFixture]
    public class EnemyTesting {

		private GameEventBus? eventBus;
		private StateMachine? stateMachine;
		private Player? testPlayer;
		private List<Image>? enemyStridesGreen;
		private List<Image>? enemyStridesRed;
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
		}

		[Test]
		public void TestEnemy() {
			if (testEnemy is null) {
				Assert.Fail();
				return;
			}
			int startHP = testEnemy.hitpoints;
			testEnemy.getShot();
			Assert.AreEqual(startHP-1, testEnemy.hitpoints);
			Assert.False(testEnemy.getShot());
		}

		[Test]
		public void TestDead() {
			if (testEnemy is null) {
				Assert.Fail();
				return;
			}
			for (int i = 0; i < 3; i++) {
				testEnemy.getShot();
			}
			Assert.True(testEnemy.getShot());
		}

		[Test]
		public void TestEnrage() {
			if (testEnemy is null) {
				Assert.Fail();
				return;
			}
			float startSpd = testEnemy.Speed;
			testEnemy.getShot();
			testEnemy.getShot();
			Assert.AreEqual(startSpd + startSpd * 2, testEnemy.Speed);
		}
	}
}
