using System;
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
		private List<Image>? enemyStridesGreen;
		private List<Image>? enemyStridesRed;
		private IMovementStrategy? testStrategy;
		private Enemy? testEnemy;
		private EntityContainer<Enemy>? EnemyContainer;

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
			testStrategy = new Down();
			EnemyContainer = new EntityContainer<Enemy>(2);
			for (int i = 0; i < 2; i++) {
				EnemyContainer.AddEntity(new Enemy(
					new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
					new ImageStride(80, enemyStridesGreen),
					new ImageStride(80, enemyStridesRed)));
			}
		}

		[Test]
		public void TestStrategyMoveEnemy() {
			if (testStrategy is null || testEnemy is null) {
				Assert.Fail();
				return;
			}
			float startPos = testEnemy.Shape.Position.Y;
			testStrategy.MoveEnemy(testEnemy);
			// We expect the enemy to be moved down equivilent to its speed.
			Assert.AreEqual(startPos-testEnemy.Speed, testEnemy.Shape.Position.Y);
		}

		[Test]
		public void TestStrategyMoveEnemies() {
			if (testStrategy is null || testEnemy is null || EnemyContainer is null) {
				Assert.Fail();
				return;
			}
			//Rounded because of float inprecision
			testStrategy.MoveEnemies(EnemyContainer);
			foreach (Enemy enemy in EnemyContainer) {
				Assert.AreEqual(MathF.Round(0.9f-testEnemy.Speed,5), MathF.Round(enemy.Shape.Position.Y, 5));
			}
		}
	}
}
