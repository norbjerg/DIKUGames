using System.Collections.Generic;
using NUnit.Framework;
using DIKUArcade.Events;
using DIKUArcade.GUI;
using Galaga.GalagaStates;

namespace GalagaTests {
	[TestFixture]
	public class StateMachineTesting {
		private GameEventBus? eventBus;
		private StateMachine? stateMachine;

		[SetUp]
		public void InitiateStateMachine() {
			Window.CreateOpenGLContext();
			eventBus = new GameEventBus();

			eventBus.InitializeEventBus(new List<GameEventType> {
				GameEventType.GameStateEvent,
				GameEventType.InputEvent,
				GameEventType.WindowEvent });

			stateMachine = new StateMachine();
			eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
			eventBus.Subscribe(GameEventType.WindowEvent, stateMachine);
    }

		[Test]
		public void TestInitialState() {
			if (stateMachine is null) {
				Assert.Fail();
				return;
			}
			Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
		}

		[Test]
		public void TestEventGamePaused() {
			if (eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			eventBus.RegisterEvent(
				new GameEvent {
					EventType = GameEventType.GameStateEvent,
					Message = "CHANGE_STATE",
					StringArg1 = "GAME_PAUSED"
				}
			);
			eventBus.ProcessEventsSequentially();
			Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
		}

		[Test]
		public void TestEventGameRunning() {
			if (eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			eventBus.RegisterEvent(
				new GameEvent {
					EventType = GameEventType.GameStateEvent,
					Message = "CHANGE_STATE",
					StringArg1 = "GAME_RUNNING"
				}
			);
			eventBus.ProcessEventsSequentially();
			Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
		}

		[Test]
		public void TestEventGameMainMenu() {
			if (eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			eventBus.RegisterEvent(
				new GameEvent {
					EventType = GameEventType.GameStateEvent,
					Message = "CHANGE_STATE",
					StringArg1 = "GAME_PAUSED"
				}
			);
			eventBus.ProcessEventsSequentially();
			Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

			eventBus.RegisterEvent(
				new GameEvent {
					EventType = GameEventType.GameStateEvent,
					Message = "CHANGE_STATE",
					StringArg1 = "MAIN_MENU"
				}
			);
			eventBus.ProcessEventsSequentially();
			Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
		}
	}
}
