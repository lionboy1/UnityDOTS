using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine.Profiling;

[BurstCompile]
public struct InsectParallelJob : IJobParallelFor
{
    public NativeArray<float3> positionArray;
    public NativeArray<float> movementArray;
    public float deltaTime;
    public int _verticalLimitPos;
    public void Execute(int index)
    {
        positionArray[index] += new float3(0, 0, movementArray[index] * deltaTime);
        if(positionArray[index].z > 5)
        {
            // ins.tr.Rotate(0, 180, 0);
            movementArray[index] = -math.abs(movementArray[index]);
        }
        if(positionArray[index].z < -5)
        {
            // ins.tr.Rotate(0, -180, 0);
            movementArray[index] = +math.abs(movementArray[index]);
        }
        float someExpensiveCalculatedValue = 0f;
        for(int i = 0; i < 1000; i++)
        {
            someExpensiveCalculatedValue = math.sqrt(173829);
        }
    }
}

public class InsectJob : MonoBehaviour
{
    public class Insect
    {
        public Transform tr;
        public float VerticalMovement;
    }
    public static List<Insect> insectList = new List<Insect>();
    [SerializeField] Transform insectPrefab;
    public int insectSpawnAmount;
    [SerializeField] int verticalLimitPos;
    public bool jobsEnabled;
    // 
    JobHandle handle;
    #region
    void Start()
    {
        for(int i = 0; i < insectSpawnAmount; i++)
        {
            Transform spawnedInsect = Instantiate(insectPrefab, new Vector2(UnityEngine.Random.Range(0.4f, 5f), UnityEngine.Random.Range(0.5f, 2f)), Quaternion.identity);
            spawnedInsect.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            insectList.Add(
                new Insect{ 
                    tr = spawnedInsect.transform,
                    VerticalMovement = UnityEngine.Random.Range(1f, 3f)
                });
        }
    }
    #endregion
    void Update()
    {
        if(jobsEnabled)
        {
            Profiler.BeginSample("Insect Profile");
            NativeArray<float3> PositionArray = new NativeArray<float3>(insectList.Count, Allocator.TempJob);
            NativeArray<float> MovementArray = new NativeArray<float>(insectList.Count, Allocator.TempJob);

            for(int i = 0; i < insectList.Count; i++)
            {
                PositionArray[i] = insectList[i].tr.position;
                MovementArray[i] = insectList[i].VerticalMovement;
            }

            InsectParallelJob theJob = new InsectParallelJob
            {
                deltaTime = Time.deltaTime,
                positionArray = PositionArray,
                movementArray = MovementArray
            };
            handle = theJob.Schedule(insectList.Count, 50);
            handle.Complete();
            //Feed back the calculted data
            for(int i = 0; i < insectList.Count; i++)
            {
                insectList[i].tr.position = PositionArray[i];
                insectList[i].VerticalMovement = MovementArray[i];
            }
            Profiler.EndSample();
            PositionArray.Dispose();
            MovementArray.Dispose();
            
        }
        else
        {
            foreach(Insect ins in insectList)
            {
                ins.tr.position += new Vector3(0, 0, ins.VerticalMovement * Time.deltaTime);
                if(ins.tr.position.z > verticalLimitPos)
                {
                    ins.tr.Rotate(0, 180, 0);
                    ins.VerticalMovement = -math.abs(ins.VerticalMovement);
                }
                if(ins.tr.position.z < -verticalLimitPos)
                {
                    ins.tr.Rotate(0, -180, 0);
                    ins.VerticalMovement = +math.abs(ins.VerticalMovement);
                }
                float someExpensiveCalculatedValue = 0f;
                for(int i = 0; i < 1000; i++)
                {
                    someExpensiveCalculatedValue = math.sqrt(173829);
                }
            }
        }
    }
}