using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Mathematics;

public partial class JobSystemScript
{
    public struct CirclingPrefab : IJobParallelForTransform
    {
        public float time;
        [ReadOnly] public NativeArray<float> speeds;
        [ReadOnly] public NativeArray<float> radii;
        [ReadOnly] public NativeArray<float3> centers;
        [ReadOnly] public NativeArray<float> startTimes;

        public void Execute(int index, TransformAccess transform)
        {
            float angle = time * speeds[index] + startTimes[index];
            float x = centers[index].x + math.cos(angle) * radii[index];
            float z = centers[index].z + math.sin(angle) * radii[index];

            transform.position = new float3(x, centers[index].y, z);
        }
    }
}