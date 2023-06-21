using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial struct CubeRotatingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Random randomValue = new(1);
        float3 moveDirection = new float3(0, -1, 0); ;

        float4 redColor = new float4(255, 0, 0, 255);
        float4 blueColor = new float4(0, 200, 255, 255);
        var cubeRotatingJob = new CubeRotatingJob { deltaTime = SystemAPI.Time.DeltaTime, blueColor=blueColor, redColor=redColor, moveDirection=moveDirection, random = randomValue};
        
        cubeRotatingJob.ScheduleParallel();
    }
}
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
