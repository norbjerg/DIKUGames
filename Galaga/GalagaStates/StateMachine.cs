using DIKUArcade.Events;
using DIKUArcade.State;
namespace Galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor {
        public IGameState ActiveState { get; private set; }
        public StateMachine() {
                
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            // GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            // Removed subscription^^ from InputEvent too this class because it does not use it
            // The subsription is moved to the 'Player' class
            ActiveState = MainMenu.GetInstance();
        }
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
                case (GameStateType.GamePaused):
                    ActiveState = GamePaused.GetInstance();
                    break;
                case (GameStateType.GameRunning):
                    ActiveState = GameRunning.GetInstance();
                    break;
                case (GameStateType.MainMenu):
                    ActiveState = MainMenu.GetInstance();
                    break;
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            this.SwitchState(StateTransformer.TransformStringToState(gameEvent.StringArg1));
            if (gameEvent.StringArg2 == "RESET") ActiveState.ResetState();
        }
    }
}
