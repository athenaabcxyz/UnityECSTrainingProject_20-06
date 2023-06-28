using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class BulletCollideSystem : SystemBase
{
    public Action<int> OnUpdateScore;
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (transform, speed, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletProperties>>().WithEntityAccess())
        {
            foreach (var (cubeTransform, hp, tag, cubeEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<CubeHP>, RefRO<CubeTag>>().WithEntityAccess())
            {
                if (math.distancesq(transform.ValueRO.Position, cubeTransform.ValueRO.Position) <= 8)
                {
                    ecb.DestroyEntity(entity);
                    hp.ValueRW.HP -= speed.ValueRO.bulletDmg;
                    if (hp.ValueRO.HP <= 0)
                    {
                        OnUpdateScore?.Invoke(tag.ValueRO.tag ? 2 : 1);
                        ecb.DestroyEntity(cubeEntity);
                    }
                    break;
                }
            }
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
