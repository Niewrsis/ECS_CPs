using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    private EcsWorld _world;
    private IEcsSystems _systems;

    void Start()
    {
        _world = new EcsWorld();

        _systems = new EcsSystems(_world);

        var providers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int providerCount = 0;
        foreach (var provider in providers)
        {
            if (provider is IECSProvider ecsProvider)
            {
                ecsProvider.Initialize(_world);
                providerCount++;
            }
        }

        _systems.Add(new CounterSystem());

        _systems.Add(new BallMovementSystem());

        _systems.Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        _systems?.Destroy();
        _systems = null;
        _world?.Destroy();
        _world = null;
    }
}