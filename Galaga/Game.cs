using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using Galaga.GalagaStates;

namespace Galaga;
public class Game : DIKUGame, IGameEventProcessor {

    private GameEventBus eventBus;
    private StateMachine stateMachine;

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        eventBus = GalagaBus.GetBus();

        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.GameStateEvent,
            GameEventType.InputEvent,
            GameEventType.WindowEvent });

        stateMachine = new StateMachine();

        eventBus.Subscribe(GameEventType.WindowEvent, this);
        window.SetKeyEventHandler(KeyHandler);
    }

    public override void Render() {
        window.Clear();
        stateMachine.ActiveState.RenderState();
    }

    public override void Update() {
        window.PollEvents();
        eventBus.ProcessEventsSequentially();
        stateMachine.ActiveState.UpdateState();

    }

    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
        stateMachine.ActiveState.HandleKeyEvent(action, key);
    }

    public void ProcessEvent(GameEvent gameEvent) {
        // Lige nu er denne case 100% af tilfældende, derfor kan den evt. reduceres
        if (gameEvent.EventType == GameEventType.WindowEvent) {
            if (gameEvent.Message == "QUIT_GAME"){
                window.CloseWindow();
            }
        }
    }
}
