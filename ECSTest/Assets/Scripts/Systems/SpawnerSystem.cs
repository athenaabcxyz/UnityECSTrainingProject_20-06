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
        state.RequireForUpdate<GameStateCommand>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Random randomValue = new Random(1337);
        Spawner config;
        if (SystemAPI.TryGetSingleton<Spawner>(out config))
        {
            var CubeQuery = SystemAPI.QueryBuilder().WithAll<CubeHP>().Build();
            if (CubeQuery.IsEmpty)
            {
                var transform = new LocalTransform();
                var ecb = new EntityCommandBuffer(Allocator.Temp);
                var Cubes = new NativeArray<Entity>(config.enemiesQuantity % 50 == 0 ? 1 : config.enemiesQuantity, Allocator.TempJob);
                ecb.Instantiate(config.CubePrefab, Cubes);
                ecb.Playback(state.EntityManager);
                if (config.enemiesQuantity % 8 == 0)
                {

                    var rectangleSpawner = new TwoSideSpawner
                    {
                        transform = transform,
                        randomValue = randomValue,
                        config = config
                    };
                    rectangleSpawner.ScheduleParallel();
                }
                else
                if (config.enemiesQuantity % 20 == 0 && config.enemiesQuantity % 40 != 0)
                {

                    var rectangleSpawner = new TriangleSpawner
                    {
                        transform = transform,
                        randomValue = randomValue,
                        config = config
                    };
                    rectangleSpawner.ScheduleParallel();
                }
                else
                if (config.enemiesQuantity % 50 == 0)
                {
                    var rectangleSpawner = new BossSpawner
                    {
                        transform = transform,
                        randomValue = randomValue,
                        config = config
                    };
                    rectangleSpawner.ScheduleParallel();
                }
                else
                {
                    var rectangleSpawner = new RectangleSpawner
                    {
                        transform = transform,
                        randomValue = randomValue,
                        config = config
                    };
                    rectangleSpawner.ScheduleParallel();
                }
                var job = new LevelUpJob();
                job.Schedule();
                ecb.Dispose();
                Cubes.Dispose();
            }
        }
    }
    public void DisableSystem(ref SystemState state)
    {
        state.Enabled = false;
    }
}




