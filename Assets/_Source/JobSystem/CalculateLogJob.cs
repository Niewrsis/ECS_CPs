using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

public partial class JobSystemScript
{
    public struct CalculateLogJob : IJob
    {
        public NativeArray<float> logResults;
        [ReadOnly] public NativeArray<uint> randomSeeds;

        public void Execute()
        {
            for (int i = 0; i < logResults.Length; i++)
            {
                Unity.Mathematics.Random rand = new Unity.Mathematics.Random(randomSeeds[i]);
                float randomValue = rand.NextFloat(1f, 100f);

                logResults[i] = math.log(randomValue);
            }
        }
    }
}