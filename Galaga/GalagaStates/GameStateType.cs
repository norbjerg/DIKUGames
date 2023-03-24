using System;

namespace Galaga.GalagaStates;
public enum GameStateType {
    GameRunning,
    GamePaused,
    MainMenu
}

public class StateTransformer {
    public static GameStateType TransformStringToState(string state) => state switch {
        "GAME_RUNNING" => GameStateType.GameRunning,
        "GAME_PAUSED" => GameStateType.GamePaused,
        "MAIN_MENU" => GameStateType.MainMenu,
        _ => throw new ArgumentException("Invalid state"),
    };

    public static string TransformStateToString(GameStateType state) => state switch {
        GameStateType.GameRunning => "GAME_RUNNING",
        GameStateType.GamePaused => "GAME_PAUSED",
        GameStateType.MainMenu => "MAIN_MENU",
        _ => throw new ArgumentException("Invalid state"),
    };

}
