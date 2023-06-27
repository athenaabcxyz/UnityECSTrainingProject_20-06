using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public class CubeSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject CubePrefab;

    class Baker : Baker<CubeSpawnerAuthoring>
    {
        public override void Bake(CubeSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            Spawner spawner = new Spawner();
            spawner.CubePrefab = GetEntity(authoring.CubePrefab, TransformUsageFlags.None);
            spawner.currentLevel = 1;
            spawner.enemiesQuantity = 10;
            spawner.modificationMoveSpeed = 1f;
            AddComponent(entity, spawner);           
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
