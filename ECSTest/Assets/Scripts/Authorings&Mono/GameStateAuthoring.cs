using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameStateAuthoring : MonoBehaviour
{
    class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameStateCommand
            {
                currentState = 0
            });
        }
    }
}
