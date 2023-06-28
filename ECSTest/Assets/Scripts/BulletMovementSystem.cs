using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(SimulationSystemGroup))]
public partial struct BulletMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, speed, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletProperties>>().WithEntityAccess())
        {
            transform.ValueRW = transform.ValueRO.RotateZ(
                    10f * SystemAPI.Time.DeltaTime);
            transform.ValueRW.Position.y += 1 * 10f * SystemAPI.Time.DeltaTime;
            if(transform.ValueRW.Position.y > 150)
            {
                ecb.DestroyEntity(entity);
                return;
            }
        }
    }
}