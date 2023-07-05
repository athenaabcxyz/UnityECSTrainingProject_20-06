using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class LevelUpdateSystem: SystemBase
{
    public Action<int> OnLevelUpdate;
    Spawner spawner;
    protected override void OnUpdate()
    {
        SystemAPI.TryGetSingleton<Spawner>(out spawner);
        OnLevelUpdate?.Invoke(spawner.currentLevel);
    }
}
