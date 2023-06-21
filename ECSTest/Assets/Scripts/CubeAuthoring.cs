using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CubeAuthoring : MonoBehaviour
{
    class CubeBaker: Baker<CubeAuthoring>
    {
        public override void Bake(CubeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CubeHP());
            AddComponent(entity, new CubeTag());
            AddComponent(entity, new CubeSpeed
            {
                speed = 1f
            });
        }
    }
}
public struct CubeHP : IComponentData
{
    public float HP;
}

public struct CubeTag : IComponentData
{
    public bool tag;
}
public struct CubeSpeed: IComponentData
{
    public float speed;
}
public struct IsPlayerDamaged : IComponentData { 
}