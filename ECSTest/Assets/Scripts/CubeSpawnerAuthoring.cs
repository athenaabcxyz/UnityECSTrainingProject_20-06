using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CubeSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject CubePrefab;

    class Baker: Baker<CubeSpawnerAuthoring>
    {
        public override void Bake(CubeSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Spawner
            {
                CubePrefab = GetEntity(authoring.CubePrefab, TransformUsageFlags.None),
                currentLevel=1,
                enemiesQuantity=10,
                modificationMoveSpeed=1f,
            });
        }
    }
}

public struct Spawner : IComponentData
{
    public Entity CubePrefab;
    public int currentLevel;
    public int enemiesQuantity;
    public float modificationMoveSpeed;
}
