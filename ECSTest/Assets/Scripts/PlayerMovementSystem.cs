using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMovementSystem: ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameStateCommand>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        GameStateCommand gameState;
        SystemAPI.TryGetSingleton<GameStateCommand>(out gameState);
        if (gameState.currentState != 1)
            return;
        var horizontalInput = Input.GetAxis("Horizontal");
        var input = new float3(horizontalInput, 0, 0) * SystemAPI.Time.DeltaTime;
        if (input.Equals(0))
            return;
        foreach(var (transform, player) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInfo>>())
        {
            float3 newPosition = transform.ValueRW.Position + input * player.ValueRO.movementSpeed;
            if (newPosition.x <= -15 || newPosition.x >= 50)
                return;
            else
                transform.ValueRW.Position = newPosition;
        }
    }
}
