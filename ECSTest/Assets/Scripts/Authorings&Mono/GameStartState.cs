using CortexDeveloper.ECSMessages.Service;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameStartState : GameBaseState
{
    public override void Enter(GameStateManager state)
    {
        state.background.gameObject.SetActive(true);
        state.startUI.SetActive(true);
        MessageBroadcaster
        .PrepareMessage()
           .AliveForOneFrame()
           .PostImmediate(World.DefaultGameObjectInjectionWorld.EntityManager,
               new GameStateChangeCommand
               {
                   currentState = 0
               });

    }
    public override void Update(GameStateManager state)
    {
        return;
    }
    public override void Exit(GameStateManager state)
    {
        state.background.gameObject.SetActive(false);
        state.startUI.SetActive(false);
    }
}
