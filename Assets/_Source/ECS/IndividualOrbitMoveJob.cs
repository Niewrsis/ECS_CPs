using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct IndividualOrbitMoveJob : IJobEntity
{
    public float ElapsedTime;

    [BurstCompile]
    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed speed,
        in Radius radius,
        in OrbitCenter center)
    {
        float angle = ElapsedTime * speed.Value;

        float x = center.Value.x + math.cos(angle) * radius.Value;
        float z = center.Value.z + math.sin(angle) * radius.Value;

        transform.Position = new float3(x, center.Value.y, z);

        transform.Rotation = quaternion.RotateY(angle);
    }
}
