using DIKUArcade.Events;

namespace Galaga;
public static class GalagaBus {
    private static GameEventBus eventBus;
    public static GameEventBus GetBus() => GalagaBus.eventBus ??= new GameEventBus();
}
