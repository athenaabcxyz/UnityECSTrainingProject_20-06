using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class CubeAuthoring : MonoBehaviour
{
    [SerializeField] Material Cheem;
    [SerializeField] Material ChadCheem;
    class CubeBaker: Baker<CubeAuthoring>
    {
        public override void Bake(CubeAuthoring authoring)
        {
            var hybridRenderer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CubeHP());
            AddComponent(entity, new CubeTag());
            AddComponent(entity, new CubeSpeed
            {
                speed = 1f
            });


            AddComponent(entity, new MaterialChanger
            {
                cheem = hybridRenderer.RegisterMaterial(authoring.Cheem),
                chadCheem = hybridRenderer.RegisterMaterial(authoring.ChadCheem),
            });
        }
    }
}



