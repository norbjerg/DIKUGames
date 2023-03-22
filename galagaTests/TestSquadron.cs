using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.GUI;
using Galaga.GalagaStates;
using Galaga.Squadron;
using Galaga;

namespace GalagaTests {
    [TestFixture]
    public class SquadronTesting {

		private GameEventBus? eventBus;
		private StateMachine? stateMachine;
		private Player? testPlayer;
		private List<Image>? enemyStridesGreen;
		private List<Image>? enemyStridesRed;
		private ISquadron? testStandardSquadron;
		private ISquadron? testDiagonalSquadron;
		private ISquadron? testCircleSquadron;
		private ISquadron? testHellSquadron;

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
			testStandardSquadron = new StandardFormation();
			testDiagonalSquadron = new DiagonalFormation();
			testCircleSquadron = new CircleFormation();
			testHellSquadron = new HellFormation();
		}

		[Test]
		public void TestStandardSquadron() {
			if (testStandardSquadron is null) {
				Assert.Fail();
				return;
			}
			foreach (Enemy enemy in testStandardSquadron.Enemies) {
				Assert.False(enemy is Enemy);
			}
			testStandardSquadron.CreateEnemies(enemyStridesGreen, enemyStridesRed);
			foreach (Enemy enemy in testStandardSquadron.Enemies) {
				Assert.True(enemy is Enemy);
			}
		}

		[Test]
		public void TestDiagonalSquadron() {
			if (testDiagonalSquadron is null) {
				Assert.Fail();
				return;
			}
			foreach (Enemy enemy in testDiagonalSquadron.Enemies) {
				Assert.False(enemy is Enemy);
			}
			testDiagonalSquadron.CreateEnemies(enemyStridesGreen, enemyStridesRed);
			foreach (Enemy enemy in testDiagonalSquadron.Enemies) {
				Assert.True(enemy is Enemy);
			}
		}

		[Test]
		public void TestCircleSquadron() {
			if (testCircleSquadron is null) {
				Assert.Fail();
				return;
			}
			foreach (Enemy enemy in testCircleSquadron.Enemies) {
				Assert.False(enemy is Enemy);
			}
			testCircleSquadron.CreateEnemies(enemyStridesGreen, enemyStridesRed);
			foreach (Enemy enemy in testCircleSquadron.Enemies) {
				Assert.True(enemy is Enemy);
			}
		}

		[Test]
		public void TestHellSquadron() {
			if (testHellSquadron is null) {
				Assert.Fail();
				return;
			}
			foreach (Enemy enemy in testHellSquadron.Enemies) {
				Assert.False(enemy is Enemy);
			}
			testHellSquadron.CreateEnemies(enemyStridesGreen, enemyStridesRed);
			foreach (Enemy enemy in testHellSquadron.Enemies) {
				Assert.True(enemy is Enemy);
			}
		}
	}
}
