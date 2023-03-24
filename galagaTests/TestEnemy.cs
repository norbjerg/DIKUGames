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
		private List<Image>? enemyStridesGreen;
		private List<Image>? enemyStridesRed;
		private Enemy? testEnemy;

		[OneTimeSetUp]
		public void OTS () {
			Window.CreateOpenGLContext();
		}

		[SetUp]
		public void Init() {
			eventBus = new GameEventBus();

			eventBus.InitializeEventBus(new List<GameEventType> {
				GameEventType.GameStateEvent,
				GameEventType.InputEvent,
				GameEventType.WindowEvent });

			stateMachine = new StateMachine();

			eventBus.Subscribe(GameEventType.WindowEvent, stateMachine);

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
			// We test if getShot reduces hp, also that it returns false when it's not dead
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
			// Health is reduced to 0 loop, then test if getShot returns true, meaning death
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
			// When enraged, the speed is incremented by "Speed * 2". Therefore we check if that is the case
			Assert.AreEqual(startSpd + startSpd * 2, testEnemy.Speed);
		}
	}
}
