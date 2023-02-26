using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using System.Collections.Generic;

namespace Galaga
{
    public class Game : DIKUGame, IGameEventProcessor
    {
        private Player player;
        private GameEventBus eventBus;

        public Game(WindowArgs windowArgs) : base(windowArgs) {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent });
            window.SetKeyEventHandler(KeyHandler);
            eventBus.Subscribe(GameEventType.InputEvent, this);
        }

        public override void Render()
        {
            window.Clear();
            player.Render();
        }

        public override void Update()
        {
            window.PollEvents();
            eventBus.ProcessEventsSequentially();
            player.Move();
        }

        private void KeyPress(KeyboardKey key) {
            // TODO: Close window if escape is pressed
            // TODO: switch on key string and set the player's move direction
            switch (key) {
                case KeyboardKey.Escape:
                    eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.WindowEvent });
                    break;
                case KeyboardKey.Left:
                    player.SetMoveLeft(true);
                    break;
                case KeyboardKey.Right:
                    player.SetMoveRight(true);
                    break;
            }
        }
        private void KeyRelease(KeyboardKey key) {
            // TODO: switch on key string and disable the player's move direction
            switch (key) {
                case KeyboardKey.Left:
                    player.SetMoveLeft(false);
                    break;
                case KeyboardKey.Right:
                    player.SetMoveRight(false);
                    break;
            }
        }
        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            // TODO: Switch on KeyBoardAction and call proper method
        }
        public void ProcessEvent(GameEvent gameEvent) {
            // Leave this empty for now
        }
    }
}
