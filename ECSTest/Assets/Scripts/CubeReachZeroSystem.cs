using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct CubeReachZeroSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transform, entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<CubeTag>().WithEntityAccess())
        {
            if (transform.ValueRO.Position.y <= 0)
            {
                foreach (var (player, entityPlayer) in SystemAPI.Query<RefRW<PlayerInfo>>().WithEntityAccess())
                {
                    ecb.AddComponent<IsPlayerDamaged>(entityPlayer);
                }
                ecb.DestroyEntity(entity);
            }
        }
    }
}