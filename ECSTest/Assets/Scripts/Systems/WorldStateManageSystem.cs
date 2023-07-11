using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial struct WorldStateManageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameStateCommand>();
    }

    public void OnUpdate(ref SystemState state)
    {
        ref var bulletMovementState = ref state.WorldUnmanaged.GetExistingSystemState<BulletMovementSystem>();
        ref var bulletSpawnerState = ref state.WorldUnmanaged.GetExistingSystemState<BulletSpawnerSystem>();
        ref var cubeRotatingState = ref state.WorldUnmanaged.GetExistingSystemState<CubeRotatingSystem>();
        ref var playerMovementState = ref state.WorldUnmanaged.GetExistingSystemState<PlayerMovementSystem>();
        GameStateCommand gameState;
        if (SystemAPI.TryGetSingleton<GameStateCommand>(out gameState))
        {
            if (gameState.currentState != 1)
            {
                bulletMovementState.Enabled = false;
                bulletSpawnerState.Enabled = false;
                cubeRotatingState.Enabled = false;
                playerMovementState.Enabled = false;
            }
            else
            {
                bulletMovementState.Enabled = true;
                bulletSpawnerState.Enabled = true;
                cubeRotatingState.Enabled = true;
                playerMovementState.Enabled = true;
            }
        }
    }
}
