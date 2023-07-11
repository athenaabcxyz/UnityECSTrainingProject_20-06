using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameEndState : GameBaseState
{
    public override void Enter(GameStateManager state)
    {
        state.endUI.SetActive(true);
        state.background.gameObject.SetActive(true);
        MessageBroadcaster
           .PrepareMessage()
               .AliveForOneFrame()
               .PostImmediate(World.DefaultGameObjectInjectionWorld.EntityManager,
                   new GameStateChangeCommand
                   {
                       currentState = 3
                   });
    }
    public override void Update(GameStateManager state)
    {
        return;
    }
    public override void Exit(GameStateManager state)
    {
        state.endUI.SetActive(false);
        state.background.gameObject.SetActive(false);
    }
}
