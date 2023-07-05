using System;
using CortexDeveloper.ECSMessages.Components;
using CortexDeveloper.ECSMessages.Service;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIInit : MonoBehaviour
{
    private static World _world;
    private SimulationSystemGroup _simulationSystemGroup;
    private LateSimulationSystemGroup _lateSimulationSystemGroup;

    private void Awake()
    {
        InitializeMessageBroadcaster();
        CreateExampleSystems();
    }

    private void OnDestroy()
    {
        if (!_world.IsCreated)
            return;

        DisposeMessageBroadcaster();
        RemoveExampleSystem();
    }

    private void InitializeMessageBroadcaster()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        _simulationSystemGroup = _world.GetOrCreateSystemManaged<SimulationSystemGroup>();
        _lateSimulationSystemGroup = _world.GetOrCreateSystemManaged<LateSimulationSystemGroup>();

        MessageBroadcaster.InitializeInWorld(_world, _lateSimulationSystemGroup);
    }

    private void DisposeMessageBroadcaster()
    {
        MessageBroadcaster.DisposeFromWorld(_world);
    }

    private void CreateExampleSystems()
    {
        _simulationSystemGroup.AddSystemToUpdateList(_world.GetOrCreateSystem<GameStateSystem>());
    }

    private void RemoveExampleSystem()
    {
        _simulationSystemGroup.RemoveSystemFromUpdateList(_world.GetExistingSystem<GameStateSystem>());
    }
}

public struct GameStateCommand: IComponentData
{
    public int currentState;
}

public struct GameStateChangeCommand: IComponentData, IMessageComponent
{
    public int currentState;
}
