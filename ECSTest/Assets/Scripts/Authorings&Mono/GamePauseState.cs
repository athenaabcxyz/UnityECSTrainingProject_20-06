using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GamePauseState : GameBaseState
{
    public override void Enter(GameStateManager state)
    {
        state.backgroundPause.gameObject.SetActive(true);
        state.pauseUI.SetActive(true);
        MessageBroadcaster
            .PrepareMessage()
                .AliveForOneFrame()
                .PostImmediate(World.DefaultGameObjectInjectionWorld.EntityManager,
                    new GameStateChangeCommand
                    {
                        currentState = 2
                    });
    }

    public override void Update(GameStateManager state)
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            state.SwitchState(state.playingState);
        }
    }

    public override void Exit(GameStateManager state)
    {
        state.backgroundPause.gameObject.SetActive(false);
        state.pauseUI.SetActive(false);
    }
}