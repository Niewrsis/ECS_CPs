using Leopotam.EcsLite;
using UnityEngine;

public class CounterProvider : MonoBehaviour, IECSProvider
{
    public int StartValue = 0;

    private EcsWorld _world;
    private int _entity;

    public void Initialize(EcsWorld world)
    {
        _world = world;
        _entity = world.NewEntity();

        var counterPool = world.GetPool<CounterComponent>();
        counterPool.Add(_entity);

        ref var counter = ref counterPool.Get(_entity);
        counter.Value = StartValue;
    }

    void OnDestroy()
    {
        if (_world != null && _world.IsAlive())
        {
            EcsPackedEntity packedEntity = _world.PackEntity(_entity);
            if (packedEntity.Unpack(_world, out _))
            {
                _world.DelEntity(_entity);
            }
        }
    }
}