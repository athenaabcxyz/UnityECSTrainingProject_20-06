using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float movementSpeed = 1f;
    [SerializeField] GameObject bulletPrefab;

    class Baker: Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerInfo
            {
                movementSpeed = authoring.movementSpeed,
                BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                HitPoint = 100
            });
        }
    }
}

public struct PlayerInfo : IComponentData
{
    public float movementSpeed;
    public Entity BulletPrefab;
    public int HitPoint;
}
