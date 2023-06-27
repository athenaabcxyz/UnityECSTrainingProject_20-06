using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = Unity.Mathematics.Random;


public partial struct SpawnerSystem : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Random randomValue = new Random(1337);
        var config = SystemAPI.GetSingleton<Spawner>();
        var CubeQuery = SystemAPI.QueryBuilder().WithAll<CubeHP>().Build();
        if (CubeQuery.IsEmpty)
        {
            var transform = new LocalTransform();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var Cubes = new NativeArray<Entity>(config.enemiesQuantity, Allocator.Temp);

            ecb.Instantiate(config.CubePrefab, Cubes);

            foreach (var cube in Cubes)
            {

                transform.Position.x = randomValue.NextInt(-20, 70);
                transform.Position.y = randomValue.NextInt(30, 40);

                bool tag = randomValue.NextBool();

                ecb.SetComponent(cube, new LocalTransform
                {
                    Position = transform.Position,
                    Scale = tag? 4:2,
                    Rotation = Quaternion.identity,
                });
                ecb.SetComponent(cube, new CubeSpeed { speed = config.modificationMoveSpeed });
                ecb.SetComponent(cube, new CubeTag { tag = tag });
                if (tag)
                {
                    ecb.SetComponent(cube, new CubeHP { HP = 10 });
                }
                else
                {
                    ecb.SetComponent(cube, new CubeHP { HP = 5 });
                }
            }
            var job = new LevelUpJob();
            job.Schedule();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}

public partial struct LevelUpJob : IJobEntity
{
    public void Execute(ref Spawner config)
    {
        config.currentLevel++;
        config.enemiesQuantity += 10;
        config.modificationMoveSpeed *= (1 + (0.1f * config.currentLevel));
    }
}


