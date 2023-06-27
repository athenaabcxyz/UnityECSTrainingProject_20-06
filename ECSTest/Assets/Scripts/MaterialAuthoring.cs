using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public partial struct MaterialAuthoring : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MaterialChanger>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (mmi, material, hp) in SystemAPI.Query<RefRW<MaterialMeshInfo>, RefRO<MaterialChanger>, RefRO<CubeHP>>())
        {
            if (hp.ValueRO.HP==10)
            {
                mmi.ValueRW.MaterialID = material.ValueRO.chadCheem;
            }
            else
            if(hp.ValueRO.HP==5)
            {
                mmi.ValueRW.MaterialID = material.ValueRO.cheem;
            }
        }
    }
}
