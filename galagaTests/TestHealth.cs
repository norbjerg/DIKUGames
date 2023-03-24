using System.Collections.Generic;
using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.GUI;
using Galaga.GalagaStates;
using Galaga;

namespace GalagaTests {
    [TestFixture]
    public class HealthTesting {

		private GameEventBus? eventBus;
		private StateMachine? stateMachine;
		private Health? testHealth;

		[OneTimeSetUp]
		public void OTS() {
			Window.CreateOpenGLContext();
		}

		[SetUp]
		public void Init() {
			eventBus = new GameEventBus();

			eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.GameStateEvent,
				GameEventType.InputEvent,
				GameEventType.WindowEvent });

			stateMachine = new StateMachine();

			eventBus.Subscribe(GameEventType.WindowEvent, stateMachine);
            testHealth = new Health(new Vec2F(0.85f, -0.2f), new Vec2F(0.25f, 0.25f));
		}

		[Test]
		public void TestLoseHealthAndUpdateHealthBuffer() {
			if (testHealth is null) {
				Assert.Fail();
				return;
			}
			Assert.AreNotEqual(testHealth, null);
			// Test that start health is 3.
			Assert.AreEqual(testHealth.health, 3);
			// Test reducing health, loseHealthBuffer preventing reducing health, and reducing loseHealthBuffer.
			for (int i = 1; i <= 3; i++) {
				testHealth.LoseHealth();
				Assert.AreEqual(testHealth.health, 3-i);
				testHealth.LoseHealth();
				Assert.AreEqual(testHealth.loseHealthBuffer, 60);
				Assert.AreEqual(testHealth.health, 3-i);
				testHealth.UpdateHealthBuffer();
				Assert.AreEqual(testHealth.loseHealthBuffer, 59);
				for (int j = 0; j < 60; j++) {
					testHealth.UpdateHealthBuffer();
				}
				Assert.AreEqual(testHealth.loseHealthBuffer, 0);
			}
			//Test if health can be reduced below 0.
			Assert.AreEqual(testHealth.health, 0);
			Assert.AreEqual(testHealth.loseHealthBuffer, 0);
			testHealth.LoseHealth();
			Assert.AreEqual(testHealth.health, 0);
		}
	}
}
