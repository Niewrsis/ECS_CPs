using Leopotam.EcsLite;

public class CounterSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsPool<CounterComponent> _counterPool;
    private EcsFilter _counterFilter;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _counterPool = world.GetPool<CounterComponent>();
        _counterFilter = world.Filter<CounterComponent>().End();

        foreach (var entity in _counterFilter)
        {
            ref var counter = ref _counterPool.Get(entity);
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _counterFilter)
        {
            ref var counter = ref _counterPool.Get(entity);
            counter.Value++;
        }
    }
}