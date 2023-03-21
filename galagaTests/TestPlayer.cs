using System;
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
    public class PlayerTesting {

		private GameEventBus? eventBus;
		private StateMachine? stateMachine;
		private Player? testPlayer;

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
		}

		[Test]
		public void TestPlayerMoveUp() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "UP PRESS"
						}
					);
			testPlayer.Move();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "UP RELEASE"
						}
					);
			// Rounded startPos.Y because of float imprecision.
			Assert.AreEqual(MathF.Round(startPos.Y + 0.01f,2), testPlayer.GetPosition().Y);
		}
		[Test]
		public void TestPlayerMoveDown() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "DOWN PRESS"
						}
					);
			testPlayer.Move();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "DOWN RELEASE"
						}
					);
			// Rounded startPos.Y because of float imprecision.
			Assert.AreEqual(MathF.Round(startPos.Y - 0.01f,2), testPlayer.GetPosition().Y);
		}

		[Test]
		public void TestPlayerMoveLeft() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "LEFT PRESS"
						}
					);
			testPlayer.Move();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "LEFT RELEASE"
						}
					);
			// Rounded startPos.X because of float imprecision.
			Assert.AreEqual(MathF.Round(startPos.X - 0.01f,2), testPlayer.GetPosition().X);
		}

		[Test]
		public void TestPlayerMoveRight() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "RIGHT PRESS"
						}
					);
			testPlayer.Move();
			testPlayer.ProcessEvent(
                    new GameEvent{
                        EventType = GameEventType.InputEvent,
                        From = this,
                        To = testPlayer,
                        Message = "RIGHT RELEASE"
						}
					);
			// Rounded startPos.X because of float imprecision.
			Assert.AreEqual(MathF.Round(startPos.X + 0.01f,2), testPlayer.GetPosition().X);
		}

		[Test]
		public void TestPlayerLeftBounds() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			for (int i = 0; i < 100; i++) {
				testPlayer.ProcessEvent(
						new GameEvent{
							EventType = GameEventType.InputEvent,
							From = this,
							To = testPlayer,
							Message = "LEFT PRESS"
							}
						);
				testPlayer.Move();
				testPlayer.ProcessEvent(
						new GameEvent{
							EventType = GameEventType.InputEvent,
							From = this,
							To = testPlayer,
							Message = "LEFT RELEASE"
							}
						);
			}
			Assert.True(testPlayer.GetPosition().X > 0);
		}

		[Test]
		public void TestPlayerRightBounds() {
			if (testPlayer is null || eventBus is null || stateMachine is null) {
				Assert.Fail();
				return;
			}
			var startPos = testPlayer.GetPosition();
			for (int i = 0; i < 100; i++) {
				testPlayer.ProcessEvent(
						new GameEvent{
							EventType = GameEventType.InputEvent,
							From = this,
							To = testPlayer,
							Message = "RIGHT PRESS"
							}
						);
				testPlayer.Move();
				testPlayer.ProcessEvent(
						new GameEvent{
							EventType = GameEventType.InputEvent,
							From = this,
							To = testPlayer,
							Message = "RIGHT RELEASE"
							}
						);
			}
			Assert.True(testPlayer.GetPosition().X < 1);
		}
	}
}
