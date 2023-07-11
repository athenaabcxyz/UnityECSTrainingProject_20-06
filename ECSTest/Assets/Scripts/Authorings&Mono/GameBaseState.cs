using UnityEngine;

public abstract class GameBaseState
{
    public abstract void Enter(GameStateManager state);
    public abstract void Update(GameStateManager state);
    public abstract void Exit(GameStateManager state);

}
