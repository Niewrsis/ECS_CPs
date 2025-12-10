using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (spawner, prefabHolder, entity) in
                 SystemAPI.Query<RefRW<Spawner>, RefRO<PrefabHolder>>()
                 .WithEntityAccess())
        {
            if (spawner.ValueRO.HasSpawned)
                continue;

            spawner.ValueRW.HasSpawned = true;

            SpawnGrid(ref state, spawner.ValueRO, prefabHolder.ValueRO.Value, ecb);

            ecb.RemoveComponent<Spawner>(entity);
        }
    }

    [BurstCompile]
    private void SpawnGrid(ref SystemState state, Spawner spawner, Entity prefab, EntityCommandBuffer ecb)
    {
        int totalCount = spawner.GridSizeX * spawner.GridSizeZ;

        float startX = -(spawner.GridSizeX - 1) * spawner.Spacing * 0.5f;
        float startZ = -(spawner.GridSizeZ - 1) * spawner.Spacing * 0.5f;

        for (int x = 0; x < spawner.GridSizeX; x++)
        {
            for (int z = 0; z < spawner.GridSizeZ; z++)
            {
                var newEntity = ecb.Instantiate(prefab);

                float3 position = new float3(
                    startX + x * spawner.Spacing,
                    0,
                    startZ + z * spawner.Spacing
                );

                var random =Unity.Mathematics.Random.CreateFromIndex((uint)(x * spawner.GridSizeZ + z));

                position.y = random.NextFloat(0f, 10f);

                ecb.SetComponent(newEntity, new OrbitCenter
                {
                    Value = position
                });

                ecb.SetComponent(newEntity, LocalTransform.FromPosition(position));

                ecb.SetComponent(newEntity, new MoveSpeed
                {
                    Value = random.NextFloat(0.5f, 3.0f)
                });

                ecb.SetComponent(newEntity, new Radius
                {
                    Value = random.NextFloat(0.5f, 3.0f)
                });
            }
        }
    }
}