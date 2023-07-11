using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
partial struct CubeRotatingJob : IJobEntity
{
    public float deltaTime;
    public Random random;
    public float3 moveDirection;
    public float4 redColor;
    public float4 blueColor;
    public Spawner spawner;
    void Execute(ref LocalTransform transform, in CubeTag tag, in CubeSpeed speed)
    {
        transform.Position.y -= 1 * deltaTime * speed.speed;
        transform.Position.z = 0;

        if (transform.Position.x <= -15)
            transform.Position.x = -15;
        if (transform.Position.x >= 50)
            transform.Position.x = 50;
    }
}
[BurstCompile]
public partial struct EnemiesDestroyJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public void Execute([EntityIndexInQuery] int index, in CubeHP cube, Entity entity)
    {
        ecb.DestroyEntity(index, entity);
    }
}

[BurstCompile]
public partial struct BulletDestroyJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public void Execute([EntityIndexInQuery] int index, in BulletProperties bullet, Entity entity)
    {
        ecb.DestroyEntity(index, entity);
    }
}

[BurstCompile]
public partial struct LevelResetJob : IJobEntity
{
    public void Execute(ref Spawner spawner)
    {
        spawner.currentLevel = 1;
        spawner.enemiesQuantity = 10;
        spawner.modificationMoveSpeed = 1f;
    }
}

[BurstCompile]
public partial struct PlayerResetJob : IJobEntity
{
    public void Execute(ref PlayerInfo player)
    {
        player.HitPoint = 100;
    }
}
[BurstCompile]
public partial struct StartGameCommandListenerJob : IJobEntity
{
    public GameStateChangeCommand stateCommand;
    public void Execute(ref GameStateCommand command)
    {
        command.currentState = stateCommand.currentState;
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
        hp = new CubeHP { HP = 100 * (config.currentLevel / 5) };
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
        transform.Position.x = -8 + 6 * (index % 10);
        transform.Position.y = (30 + 6 * (index / 10));
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
public partial struct TriangleSpawner : IJobEntity
{
    public LocalTransform transform;
    public Random randomValue;
    public Spawner config;
    public void Execute([EntityIndexInQuery] int index, ref LocalTransform cubeTransform, ref CubeTag tag, ref CubeHP hp, ref CubeSpeed speed)
    {
        int currentPos = index % 20;
        if (currentPos <= 7)
        {
            transform.Position.x = -2 + 6 * (currentPos % 8);
            transform.Position.y = (30 + 24 * (index / 20));
        }
        else
            if (currentPos <= 13)
        {
            transform.Position.x = 4 + 6 * ((currentPos - 8) % 6);
            transform.Position.y = (36 + 24 * (index / 20));
        }
        else
           if (currentPos <= 17)
        {
            transform.Position.x = 10 + 6 * ((currentPos - 14) % 4);
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
            transform.Position.x = 4 - ((index / 8) % 2) * 6 + 6 * (index % 8);
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

