using UnityEngine.Jobs;
using UnityEngine.Profiling;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

public class BugsJob : MonoBehaviour
{
    [SerializeField] Transform Prefab;
    [Range(1, 3000)]
    public int _spawnAmount;
    public List<Bug> _bugsList = new List<Bug>();
    public bool _job;
    
    public class Bug
    {
        public Transform transform;
        public float move;
    }

    void Start()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            Transform bug = Instantiate(Prefab, new Vector3(UnityEngine.Random.Range(-3, 6), UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(-4, 4)), Quaternion.identity);
            _bugsList.Add(new Bug
            {
                transform = bug,
                move = UnityEngine.Random.Range(1f, 2.5f)
            });
        }
    }
    void Update()
    {
        if (_job)
        {
            Profiler.BeginSample("Bug Job");
            NativeArray<float3> finalPosition = new NativeArray<float3>(_bugsList.Count, Allocator.TempJob);
            NativeArray<float> finalMovement = new NativeArray<float>(_bugsList.Count, Allocator.TempJob);
            //Populate arrays aboove with data to give to the job
            for(int i = 0; i < _bugsList.Count; i++)
            {
                finalPosition[i] = _bugsList[i].transform.position;
                finalMovement[i] = _bugsList[i].move;
            }

            //Pass the filled array data from above to the new job
            
            BugJobJobify bugJob = new BugJobJobify
            {
                Position = finalPosition,
                Movement = finalMovement,
                deltaTime = Time.deltaTime
            };

            JobHandle jHandle = bugJob.Schedule(_bugsList.Count, 50);


            jHandle.Complete();

            for(int i = 0; i < _bugsList.Count; i++)
            {
                _bugsList[i].transform.position = finalPosition[i];
                _bugsList[i].move =  finalMovement[i];
            }
            Debug.Log("Job Complete");
            finalPosition.Dispose();
            finalMovement.Dispose();
            Profiler.EndSample();
        }
        else
        {
            SlowWay();
        }
    }

    void SlowWay()
    {
        foreach (Bug bug in _bugsList)
        {
            bug.transform.position += new Vector3(0, 0, bug.move * Time.deltaTime);
            if (bug.transform.position.z > 10)
            {
                bug.move = -math.abs(bug.move);
            }
            if (bug.transform.position.z < -10)
            {
                bug.move = +math.abs(bug.move);
            }
            float _randomExpensiveResult;
            for (int i = 0; i < 2000; i++)
            {
                _randomExpensiveResult = math.sqrt(234000);
            }
        }
        
    }
    [BurstCompile]
    public struct BugJobJobify : IJobParallelFor
    {
        public NativeArray<float3> Position;
        public NativeArray<float> Movement;
        public float deltaTime; 
        public void Execute(int index)
        {
   
            Position[index] += new float3(0, 0, Movement[index] * deltaTime);
            if (Position[index].z > 10)
            {
                Movement[index] = -math.abs(Movement[index]);
            }
            if (Position[index].z < -10)
            {
                Movement[index] = +math.abs(Movement[index]);
            }

            float _randomExpensiveResult;
            for (int i = 0; i < 2000; i++)
            {
                _randomExpensiveResult = math.sqrt(234000);
            }
        }
    }
}