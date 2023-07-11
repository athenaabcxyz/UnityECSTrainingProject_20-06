using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct GameResetSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OnGameResetTrigger>();
    }
    public void OnUpdate(ref SystemState state)
    {
        foreach(var isReset in SystemAPI.Query<RefRO<OnGameResetTrigger>>())
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob).AsParallelWriter();
            var EnemiesDestroy = new EnemiesDestroyJob { ecb = ecb };
            var BulletDestroy = new BulletDestroyJob { ecb = ecb };
            var ResetPlayer = new PlayerResetJob();
            var SpawnerReset = new LevelResetJob();

            EnemiesDestroy.ScheduleParallel();
            BulletDestroy.ScheduleParallel();
            SpawnerReset.Schedule(state.Dependency);
            state.Dependency.Complete();
            ResetPlayer.Schedule(state.Dependency);
            state.Dependency.Complete();
        }
    }
}

