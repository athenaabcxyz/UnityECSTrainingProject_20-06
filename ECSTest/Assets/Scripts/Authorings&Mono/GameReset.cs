using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameReset : MonoBehaviour
{
    [SerializeField] GameObject EndUI;
    [SerializeField] GameObject StartUI;
    public void ResetTheScene()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EndUI.SetActive(false);
        StartUI.SetActive(true);
        MessageBroadcaster
        .PrepareMessage()
            .AliveForOneFrame()
            .PostImmediate(entityManager, new OnGameResetTrigger());
        MessageBroadcaster
        .PrepareMessage()
            .AliveForOneFrame()
            .PostImmediate(entityManager, new GameStateChangeCommand { currentState=0});
    }
}
