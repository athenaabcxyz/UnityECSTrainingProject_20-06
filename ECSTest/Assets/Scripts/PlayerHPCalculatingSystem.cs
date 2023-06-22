using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class PlayerHPCalculatingSystem : SystemBase
{
    public Action<int> OnUpdateHP;
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach (var (player, entity) in SystemAPI.Query<RefRW<PlayerInfo>>().WithAll<IsPlayerDamaged>().WithEntityAccess())
        {
            player.ValueRW.HitPoint--;
            OnUpdateHP?.Invoke(player.ValueRO.HitPoint);
            ecb.RemoveComponent<IsPlayerDamaged>(entity);
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
