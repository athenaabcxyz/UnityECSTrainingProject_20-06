using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float bulletSpeed = 50f;

    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BulletProperties
            {
                bulletSpeed = authoring.bulletSpeed,
                bulletDmg = 5
            });
        }
    }
}

public struct BulletProperties : IComponentData
{
    public float bulletSpeed;
    public float bulletDmg;
}
