using NUnit.Framework;
using Galaga.GalagaStates;

namespace GalagaTests;
public class GameStateTypeTest {
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void TransformStringToStateTest() {
        Assert.AreEqual(GameStateType.GameRunning, StateTransformer.TransformStringToState(
            "GAME_RUNNING"));
        Assert.AreEqual(GameStateType.GamePaused, StateTransformer.TransformStringToState("GAME_PAUSED"));
        Assert.AreEqual(GameStateType.MainMenu, StateTransformer.TransformStringToState("MAIN_MENU"));
    }

    [Test]
    public void TransformStateToStringTest() {
        Assert.AreEqual("GAME_RUNNING", StateTransformer.TransformStateToString(GameStateType.GameRunning));
        Assert.AreEqual("GAME_PAUSED", StateTransformer.TransformStateToString(GameStateType.GamePaused));
        Assert.AreEqual("MAIN_MENU", StateTransformer.TransformStateToString(GameStateType.MainMenu));
    }
}