using Unity.Entities;
using UnityEngine;

public class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new Spawner
        {
            GridSizeX = authoring.GridSizeX,
            GridSizeZ = authoring.GridSizeZ,
            Spacing = authoring.Spacing,
            HasSpawned = false
        });

        AddComponent(entity, new PrefabHolder
        {
            Value = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
        });
    }
}
