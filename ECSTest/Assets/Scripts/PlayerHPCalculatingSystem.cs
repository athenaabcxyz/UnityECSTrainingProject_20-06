using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial struct PlayerHPCalculatingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (player, entity) in SystemAPI.Query<RefRW<PlayerInfo>>().WithAll<IsPlayerDamaged>().WithEntityAccess())
        {
            player.ValueRW.HitPoint--;
            ecb.RemoveComponent<IsPlayerDamaged>(entity);
        }
    }
}
