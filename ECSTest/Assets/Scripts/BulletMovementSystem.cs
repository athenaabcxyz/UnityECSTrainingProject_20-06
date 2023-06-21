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
            transform.ValueRW.Position.y += 1 * speed.ValueRO.bulletSpeed * SystemAPI.Time.DeltaTime;
            if(transform.ValueRW.Position.y > 150)
            {
                ecb.DestroyEntity(entity);
                return;
            }

            foreach(var (cubeTransform, hp, cubeEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<CubeHP>>().WithAll<CubeTag>().WithEntityAccess())
            {
                if (math.distancesq(transform.ValueRO.Position, cubeTransform.ValueRO.Position) <= 2)
                {
                    ecb.DestroyEntity(entity);
                    hp.ValueRW.HP -= speed.ValueRO.bulletDmg;
                    if(hp.ValueRO.HP<=0)
                    ecb.DestroyEntity(cubeEntity);
                    return;
                }
            }
        }
    }
}