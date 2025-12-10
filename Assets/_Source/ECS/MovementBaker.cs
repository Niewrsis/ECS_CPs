using Unity.Entities;
using Unity.Transforms;

public class MovementBaker : Baker<ComponentAuthoring>
{
    public override void Bake(ComponentAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new OrbitCenter
        {
            Value = authoring.transform.position
        });

        AddComponent(entity, new LocalTransform
        {
            Position = authoring.transform.position,
            Rotation = authoring.transform.rotation,
            Scale = authoring.transform.localScale.x
        });

        AddComponent(entity, new MoveSpeed
        {
            Value = authoring.MoveSpeed
        });

        AddComponent(entity, new Radius
        {
            Value = authoring.Radius
        });
    }
}
