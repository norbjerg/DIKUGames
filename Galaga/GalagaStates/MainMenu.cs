using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace Galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
    
        public static MainMenu GetInstance() {
            if (MainMenu.instance == null) {
                MainMenu.instance = new MainMenu();
                MainMenu.instance.InitializeGameState();
            }
            return MainMenu.instance;
        }

        public void InitializeGameState() {
            menuButtons = new Text[] {new Text("New Game",
                new Vec2F(0.5f, 0.4f), new Vec2F(0.25f, 0.25f)),
                new Text("Quit", new Vec2F(0.5f, 0.6f), new Vec2F(0.25f, 0.25f))};
            activeMenuButton = 0;
            maxMenuButtons = 2;
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key)
        {
            if (action == KeyboardAction.KeyPress) {
                if (activeMenuButton >= 0 && activeMenuButton < maxMenuButtons) {
                    switch (key) {
                        case (KeyboardKey.Up):
                            activeMenuButton -= (activeMenuButton > 0) ? 1 : 0;
                            break;
                        case (KeyboardKey.Down):
                            activeMenuButton += (activeMenuButton < maxMenuButtons - 1) ? 1 : 0;
                            break;
                        case (KeyboardKey.Enter):
                            if (activeMenuButton == 0) {
                                GalagaBus.GetBus().RegisterEvent(
                                    new GameEvent{
                                        EventType = GameEventType.GameStateEvent,
                                        Message = "CHANGE_STATE",
                                        StringArg1 = "GAME_RUNNING"
                                    }
                                );
                            }
                            else if (activeMenuButton == 1) {
                                GalagaBus.GetBus().RegisterEvent(
                                    new GameEvent{
                                        EventType = GameEventType.GameStateEvent,
                                        Message = "QUIT_GAME",
                                        StringArg1 = "CLOSE_WINDOW"
                                    }
                                );
                            }
                            break;
                            
                    }
                }
            }
        }

        public void RenderState()
        {
            backGroundImage.RenderEntity();
            foreach (Text menuButton in menuButtons) {
                menuButton.RenderText();
            }
        }

        public void ResetState()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
}
