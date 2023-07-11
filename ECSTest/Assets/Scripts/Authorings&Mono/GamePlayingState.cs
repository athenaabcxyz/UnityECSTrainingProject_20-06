using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GamePlayingState : GameBaseState
{
    [SerializeField] GameObject gameObject;
    public override void Enter(GameStateManager state)
    {
        state.playingUI.SetActive(true);
        MessageBroadcaster
            .PrepareMessage()
                .AliveForOneFrame()
                .PostImmediate(World.DefaultGameObjectInjectionWorld.EntityManager,
                    new GameStateChangeCommand
                    {
                        currentState = 1
                    });
    }
    public override void Update(GameStateManager state)
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            state.SwitchState(state.pauseState);
            
        }
        
    }
    public override void Exit(GameStateManager state)
    {
        state.playingUI.SetActive(false);
    }

}
