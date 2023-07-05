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
        SystemAPI.TryGetSingleton<Spawner>(out config);
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

[BurstCompile]
public partial struct LevelUpJob : IJobEntity
{
    public void Execute(ref Spawner config)
    {
        config.currentLevel++;
        config.enemiesQuantity += 10;
        if (config.currentLevel % 5 == 0)
            config.modificationMoveSpeed = 1f;
        else
            config.modificationMoveSpeed += 0.1f;
    }
}

[BurstCompile]
public partial struct BossSpawner : IJobEntity
{
    public LocalTransform transform;
    public Random randomValue;
    public Spawner config;
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform cubeTransform, ref CubeTag tag, ref CubeHP hp, ref CubeSpeed speed)
    {
        transform.Position.x = 19;
        transform.Position.y = 30;

        cubeTransform = new LocalTransform
        {
            Position = transform.Position,
            Scale = 12,
            Rotation = Quaternion.LookRotation(new float3(0, 0, -1), new float3(0, 1, 0)),
        };
        speed = new CubeSpeed { speed = 1f };
        tag = new CubeTag { tag = true };
        hp = new CubeHP { HP = 100*(config.currentLevel/5) };
    }
}


[BurstCompile]
public partial struct RectangleSpawner : IJobEntity
{
    public LocalTransform transform;
    public Random randomValue;
    public Spawner config;
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform cubeTransform, ref CubeTag tag, ref CubeHP hp, ref CubeSpeed speed)
    {
        transform.Position.x = -8 + 6 * (index%10);
        transform.Position.y = (30 + 6*(index/10));
        bool isTag = randomValue.NextBool();

        cubeTransform = new LocalTransform
        {
            Position = transform.Position,
            Scale = 4,
            Rotation = Quaternion.LookRotation(new float3(0,0,-1),new float3(0,1,0)),
        };
        speed = new CubeSpeed { speed = config.modificationMoveSpeed };
        tag = new CubeTag { tag = isTag };
        if (isTag)
        {
            hp = new CubeHP { HP = 10 };
        }
        else
        {
            hp = new CubeHP { HP = 5 };
        }
    }
}

[BurstCompile]
public partial struct TriangleSpawner : IJobEntity
{
    public LocalTransform transform;
    public Random randomValue;
    public Spawner config;
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform cubeTransform, ref CubeTag tag, ref CubeHP hp, ref CubeSpeed speed)
    {
        int currentPos = index %20;
        if(currentPos<=7)
        {
            transform.Position.x = -2 + 6 * (currentPos % 8);
            transform.Position.y = (30 + 24 * (index/20));
        }
        else
            if(currentPos<=13)
        {
            transform.Position.x = 4 + 6 * ((currentPos-8) % 6);
            transform.Position.y = (36 + 24 * (index / 20));
        }
        else
           if(currentPos<=17)
        {
            transform.Position.x = 10 + 6 * ((currentPos-14) % 4);
            transform.Position.y = (42 + 24 * (index / 20));
        }
        else
        {
            transform.Position.x = 16 + 6 * ((currentPos - 18) % 2);
            transform.Position.y = (48 + 24 * (index / 20));
        }
        bool isTag = randomValue.NextBool();

        cubeTransform = new LocalTransform
        {
            Position = transform.Position,
            Scale = 4,
            Rotation = Quaternion.LookRotation(new float3(0, 0, -1), new float3(0, 1, 0)),
        };
        speed = new CubeSpeed { speed = config.modificationMoveSpeed };
        tag = new CubeTag { tag = isTag };
        if (isTag)
        {
            hp = new CubeHP { HP = 10 };
        }
        else
        {
            hp = new CubeHP { HP = 5 };
        }
    }
}

[BurstCompile]
public partial struct TwoSideSpawner : IJobEntity
{
    public LocalTransform transform;
    public Random randomValue;
    public Spawner config;
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform cubeTransform, ref CubeTag tag, ref CubeHP hp, ref CubeSpeed speed)
    {

        if (index % 8 >= 3)
        {
            transform.Position.x =  4- ((index / 8) % 2) * 6 + 6 * (index % 8);
            transform.Position.y = (30 + 6 * (index / 8));
        }
        else
        {
            transform.Position.x = -8 - ((index / 8) % 2) * 6 + 6 * (index % 8);
            transform.Position.y = (30 + 6 * (index / 8));
        }
        bool isTag = randomValue.NextBool();

        cubeTransform = new LocalTransform
        {
            Position = transform.Position,
            Scale = -4,
            Rotation = Quaternion.LookRotation(new float3(0, 0, -1), new float3(0, 1, 0)),
        };
        speed = new CubeSpeed { speed = config.modificationMoveSpeed };
        tag = new CubeTag { tag = isTag };
        if (isTag)
        {
            hp = new CubeHP { HP = 10 };
        }
        else
        {
            hp = new CubeHP { HP = 5 };
        }
    }
}



