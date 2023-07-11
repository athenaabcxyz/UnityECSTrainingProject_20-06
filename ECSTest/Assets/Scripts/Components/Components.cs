using CortexDeveloper.ECSMessages.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

public struct BulletProperties : IComponentData
{
    public float bulletSpeed;
    public float bulletDmg;
}
public struct CubeHP : IComponentData
{
    public float HP;
}

public struct CubeTag : IComponentData
{
    public bool tag;
}
public struct CubeSpeed : IComponentData
{
    public float speed;
}
public struct IsPlayerDamaged : IComponentData
{
    public int damage;
}
public struct MaterialChanger : IComponentData
{
    public BatchMaterialID cheem;
    public BatchMaterialID chadCheem;
}
public struct Spawner : IComponentData
{
    public Entity CubePrefab;
    public int currentLevel;
    public int enemiesQuantity;
    public float modificationMoveSpeed;
}

public struct PlayerInfo : IComponentData
{
    public float movementSpeed;
    public Entity BulletPrefab;
    public int HitPoint;
}
public struct GameStateCommand : IComponentData
{
    public int currentState;
}

public struct GameStateChangeCommand : IComponentData, IMessageComponent
{
    public int currentState;
    
}
public struct OnGameResetTrigger : IComponentData, IMessageComponent
{

}
public struct IsGameEndedCommand : IComponentData, IMessageComponent
{

}
