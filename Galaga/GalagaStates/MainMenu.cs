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
        private readonly Vec3I SELECTED_COLOR = new Vec3I(0,230,0);
        private readonly Vec3I UNSELECTED_COLOR = new Vec3I(255,255,255);
    
        public static MainMenu GetInstance() {
            if (MainMenu.instance == null) {
                MainMenu.instance = new MainMenu();
                MainMenu.instance.InitializeGameState();
            }
            return MainMenu.instance;
        }

        public void InitializeGameState() {
            menuButtons = new Text[] {
                new Text("New Game", new Vec2F(0.1f, 0.6f), new Vec2F(0.25f, 0.25f)),
                new Text("Quit", new Vec2F(0.1f, 0.7f), new Vec2F(0.25f, 0.25f))};

            foreach (Text button in menuButtons)
                button.SetColor(UNSELECTED_COLOR);
            this.updateButton(true);

            activeMenuButton = 0;
            maxMenuButtons = menuButtons.Length-1;

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                this.updateButton(false);

                if (activeMenuButton < 0 || activeMenuButton > maxMenuButtons)
                    return;
                
                switch (key) {
                    case KeyboardKey.Escape:
                        GalagaBus.GetBus().RegisterEvent(
                            new GameEvent{
                                EventType = GameEventType.WindowEvent,
                                Message = "QUIT_GAME",
                            }
                        );
                    break;
                    case (KeyboardKey.Up):
                        if (activeMenuButton < maxMenuButtons) activeMenuButton++;
                        break;
                    case (KeyboardKey.Down):
                        if (activeMenuButton > 0) activeMenuButton--;
                        break;
                    case (KeyboardKey.Enter):
                        if (activeMenuButton == 0) {
                            GalagaBus.GetBus().RegisterEvent(
                                new GameEvent{
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_RUNNING",
                                    StringArg2 = "RESET"
                                }
                            );
                        }
                        else if (activeMenuButton == 1) {
                            GalagaBus.GetBus().RegisterEvent(
                                new GameEvent{
                                    EventType = GameEventType.WindowEvent,
                                    Message = "QUIT_GAME",
                                }
                            );
                        }
                        break;
                }
                this.updateButton(true);
            }
        }

        private void updateButton(bool active) {
            if (active) menuButtons[activeMenuButton].SetColor(SELECTED_COLOR);
            else menuButtons[activeMenuButton].SetColor(UNSELECTED_COLOR);
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (Text menuButton in menuButtons) {
                menuButton.RenderText();
            }
        }

        public void ResetState() {
            throw new System.NotImplementedException();
        }

        public void UpdateState() {
            return;
        }
    }
}
