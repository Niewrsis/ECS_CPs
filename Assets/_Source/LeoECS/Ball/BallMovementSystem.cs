using Leopotam.EcsLite;
using UnityEngine;

public class BallMovementSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter<BallTransform>()
                         .Inc<BallMovement>()
                         .End();

        var transformPool = world.GetPool<BallTransform>();
        var movementPool = world.GetPool<BallMovement>();
        var visualPool = world.GetPool<BallVisual>();

        float deltaTime = Time.deltaTime;

        foreach (var entity in filter)
        {
            ref var transform = ref transformPool.Get(entity);
            ref var movement = ref movementPool.Get(entity);

            movement.CurrentTime += deltaTime * movement.ZigzagFrequency;

            float forwardMovement = movement.ForwardSpeed * deltaTime;
            float sideMovement = Mathf.Sin(movement.CurrentTime) * movement.ZigzagAmplitude;

            transform.Position += new Vector3(sideMovement, 0f, forwardMovement);

            if (visualPool.Has(entity))
            {
                ref var visual = ref visualPool.Get(entity);
                if (visual.GameObject != null)
                {
                    visual.GameObject.transform.position = transform.Position;
                }
            }
        }
    }
}