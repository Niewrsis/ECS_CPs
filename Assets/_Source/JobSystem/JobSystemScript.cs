using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public partial class JobSystemScript : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int prefabCount;

    [Space]
    [SerializeField] private int minRndX;
    [SerializeField] private int maxRndX, minRndY, maxRndY, minRndZ, maxRndZ;

    [Space]
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float minRadius = 1f;
    [SerializeField] private float maxRadius = 5f;

    [Space]
    [SerializeField] private float logCalculationInterval = 2f;

    private TransformAccessArray transformAccessArray;
    private NativeArray<float> speeds;
    private NativeArray<float> radii;
    private NativeArray<float3> centers;
    private NativeArray<float> startTimes;
    private NativeArray<float> logResults;
    private NativeArray<uint> randomSeeds;

    private float logTimer;
    private JobHandle logJobHandle;

    private void Start()
    {
        var transforms = new Transform[prefabCount];
        speeds = new NativeArray<float>(prefabCount, Allocator.Persistent);
        radii = new NativeArray<float>(prefabCount, Allocator.Persistent);
        centers = new NativeArray<float3>(prefabCount, Allocator.Persistent);
        startTimes = new NativeArray<float>(prefabCount, Allocator.Persistent);
        logResults = new NativeArray<float>(prefabCount, Allocator.Persistent);
        randomSeeds = new NativeArray<uint>(prefabCount, Allocator.Persistent);

        for (int i = 0; i < prefabCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(minRndX, maxRndX), 
                Random.Range(minRndY, maxRndY), 
                Random.Range(minRndZ, maxRndZ));

            transforms[i] = Instantiate(prefab, randomPos, Quaternion.identity).transform;

            speeds[i] = Random.Range(minSpeed, maxSpeed);
            radii[i] = Random.Range(minRadius, maxRadius);
            centers[i] = randomPos;
            startTimes[i] = Random.Range(0f, 6.28f);
            logResults[i] = 0f;
            randomSeeds[i] = (uint)Random.Range(1, 100000);
        }

        transformAccessArray = new TransformAccessArray(transforms);
        logTimer = logCalculationInterval;
    }

    private void Update()
    {
        CirclingPrefab movementJob = new CirclingPrefab
        {
            time = Time.time,
            speeds = speeds,
            radii = radii,
            centers = centers,
            startTimes = startTimes
        };

        JobHandle movementJobHandle = movementJob.Schedule(transformAccessArray);

        logTimer -= Time.deltaTime;
        if (logTimer <= 0f)
        {
            if (logJobHandle.IsCompleted)
            {
                logJobHandle.Complete();

                for (int i = 0; i < randomSeeds.Length; i++)
                {
                    randomSeeds[i] = (uint)Random.Range(1, 100000);
                }

                CalculateLogJob logJob = new CalculateLogJob
                {
                    logResults = logResults,
                    randomSeeds = randomSeeds
                };

                logJobHandle = logJob.Schedule();
            }

            logTimer = logCalculationInterval;
        }

        movementJobHandle.Complete();
        logJobHandle.Complete();
    }

    private void OnDestroy()
    {
        if (logJobHandle.IsCompleted) logJobHandle.Complete();

        if (transformAccessArray.isCreated)
            transformAccessArray.Dispose();
        if (speeds.IsCreated)
            speeds.Dispose();
        if (radii.IsCreated)
            radii.Dispose();
        if (centers.IsCreated)
            centers.Dispose();
        if (startTimes.IsCreated)
            startTimes.Dispose();
        if (logResults.IsCreated)
            logResults.Dispose();
        if (randomSeeds.IsCreated)
            randomSeeds.Dispose();
    }
}