using CortexDeveloper.ECSMessages.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial class GameEnderSystem : SystemBase
{
    public Action OnGameOver;
    protected override void OnUpdate()
    {
        PlayerInfo player;
        SystemAPI.TryGetSingleton<PlayerInfo>(out player);
        if (player.HitPoint <= 0)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            OnGameOver?.Invoke();
            MessageBroadcaster
           .PrepareMessage()
           .AliveForOneFrame()
           .PostImmediate(entityManager,
               new GameStateChangeCommand
               {
                   currentState = 3
               });
        }
    }
}

