using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace Galaga.GalagaStates;
public class GamePaused : IGameState {
    private static GamePaused instance = null;
    private Text[] buttons;
    private int activeButton;
    private int maxButtons;
    private readonly Vec3I SELECTED_COLOR = new Vec3I(0,230,0);
    private readonly Vec3I UNSELECTED_COLOR = new Vec3I(255,255,255);
    
    public static GamePaused GetInstance()  {
        if (GamePaused.instance == null) {
            GamePaused.instance = new GamePaused();
            GamePaused.instance.InitializeGameState();
        }
        return GamePaused.instance;
    }

    public void InitializeGameState() {
        buttons = new Text[] {
            new Text("Return to game", new Vec2F(0.1f, 0.4f), new Vec2F(0.25f, 0.25f)),
            new Text("Quit to menu", new Vec2F(0.1f, 0.5f), new Vec2F(0.25f, 0.25f)),
            new Text("Quit application", new Vec2F(0.1f, 0.6f), new Vec2F(0.25f, 0.25f))};

        foreach (Text button in buttons)
            button.SetColor(UNSELECTED_COLOR);
        this.updateButton(true);

        activeButton = 0;
        maxButtons = buttons.Length-1;
    }

    
    private void updateButton(bool active) {
        if (active) buttons[activeButton].SetColor(SELECTED_COLOR);
        else buttons[activeButton].SetColor(UNSELECTED_COLOR);
    }


    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        if (action == KeyboardAction.KeyPress) {
            this.updateButton(false);

            if (activeButton < 0 || activeButton > maxButtons)
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
                    if (activeButton < maxButtons) activeButton++;
                    break;
                case (KeyboardKey.Down):
                    if (activeButton > 0) activeButton--;
                    break;
                case (KeyboardKey.Enter):
                    if (activeButton == 0) {
                        GalagaBus.GetBus().RegisterEvent(
                            new GameEvent{
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_RUNNING"
                            }
                        );
                    }
                    else if (activeButton == 1) {
                        GalagaBus.GetBus().RegisterEvent(
                            new GameEvent{
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "MAIN_MENU"
                            }
                        );
                    }
                    else if (activeButton == 2) {
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

    public void RenderState() {
        foreach (Text button in buttons){
            button.RenderText();
        }
    }

    public void ResetState() {
        throw new System.NotImplementedException();
    }

    public void UpdateState() {
        return;
    }
}
