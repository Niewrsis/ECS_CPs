using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct IndividualOrbitMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new IndividualOrbitMoveJob
        {
            ElapsedTime = (float)SystemAPI.Time.ElapsedTime
        };

        job.ScheduleParallel();
    }
}