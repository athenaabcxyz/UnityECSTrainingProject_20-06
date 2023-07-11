using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public partial struct CubeReachZeroSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int damage = 0;
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transform, entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<CubeTag>().WithEntityAccess())
        {
            if (transform.ValueRO.Position.y < 0)
            {
                damage++;
                ecb.DestroyEntity(entity);
            }
        }
        if (damage > 0)
        {
            foreach (var (player, entityPlayer) in SystemAPI.Query<RefRW<PlayerInfo>>().WithEntityAccess())
            {
                ecb.AddComponent(entityPlayer, new IsPlayerDamaged { damage = damage });
            }
        }
        
    }
}