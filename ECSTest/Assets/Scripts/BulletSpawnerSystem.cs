using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletSpawnerSystem: ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (var (transform, player) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInfo>>())
            {
                var bullet = state.EntityManager.Instantiate(player.ValueRO.BulletPrefab);
                state.EntityManager.SetComponentData(bullet, new LocalTransform
                {
                    Position = transform.ValueRO.Position + new float3(0, 3, 0),
                    Scale = 1f,
                    Rotation = Quaternion.identity
                });
            }
        }
        
    }
}