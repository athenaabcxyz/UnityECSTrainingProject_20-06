using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public partial struct GameStateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameStateChangeCommand>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var currentState in SystemAPI.Query<RefRO<GameStateChangeCommand>>())
        {
            var job = new StartGameCommandListenerJob { stateCommand = currentState.ValueRO };
            job.ScheduleParallel();
        }
    }
}
