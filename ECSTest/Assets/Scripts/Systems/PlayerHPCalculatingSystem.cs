using CortexDeveloper.ECSMessages.Components;
using CortexDeveloper.ECSMessages.Service;
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
        foreach (var (player, damage, entity) in SystemAPI.Query<RefRW<PlayerInfo>, RefRO<IsPlayerDamaged>>().WithEntityAccess())
        {
            player.ValueRW.HitPoint -= damage.ValueRO.damage;
            OnUpdateHP?.Invoke(player.ValueRO.HitPoint);
            ecb.RemoveComponent<IsPlayerDamaged>(entity);
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}

