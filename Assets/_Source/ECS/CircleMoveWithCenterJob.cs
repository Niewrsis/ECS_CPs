using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct CircleMoveWithCenterJob : IJobEntity
{
    public float DeltaTime;
    public float ElapsedTime;

    public void Execute(ref LocalTransform transform, in MoveSpeed speed, in Radius radius)
    {
        float angle = ElapsedTime * speed.Value;

        float x = math.cos(angle) * radius.Value;
        float z = math.sin(angle) * radius.Value;

        transform.Position = new float3(x, transform.Position.y, z);
        transform.Rotation = quaternion.RotateY(angle);
    }
}