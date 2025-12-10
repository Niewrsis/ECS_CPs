using Leopotam.EcsLite;
using UnityEngine;

public class BallSpawnerProvider : MonoBehaviour, IECSProvider
{
    [Header("Settings")]
    public int BallCount = 1000;
    public float SpawnRadius = 20f;

    [Header("Parameters")]
    public float ForwardSpeed = 5f;
    public float ZigzagAmplitude = 2f;
    public float ZigzagFrequency = 2f;

    public void Initialize(EcsWorld world)
    {
        var transformPool = world.GetPool<BallTransform>();
        var movementPool = world.GetPool<BallMovement>();

        for (int i = 0; i < BallCount; i++)
        {
            int entity = world.NewEntity();

            transformPool.Add(entity);
            ref var transform = ref transformPool.Get(entity);
            transform.Position = new Vector3(
                Random.Range(-SpawnRadius, SpawnRadius),
                0,
                Random.Range(-SpawnRadius, SpawnRadius)
            );

            movementPool.Add(entity);
            ref var movement = ref movementPool.Get(entity);
            movement.ForwardSpeed = ForwardSpeed;
            movement.ZigzagAmplitude = ZigzagAmplitude;
            movement.ZigzagFrequency = ZigzagFrequency;
            movement.CurrentTime = Random.Range(0f, 10f);

            CreateVisualBall(world, entity, transform.Position);
        }
    }

    private void CreateVisualBall(EcsWorld world, int entity, Vector3 position)
    {
        GameObject visualBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualBall.name = $"Ball_{entity}";
        visualBall.transform.position = position;
        visualBall.transform.localScale = Vector3.one * 0.3f;

        var visualPool = world.GetPool<BallVisual>();
        visualPool.Add(entity);
        ref var visual = ref visualPool.Get(entity);
        visual.GameObject = visualBall;
    }
}