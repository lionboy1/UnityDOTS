using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public GameObject Prefab;
    [SerializeField] int amount;
    List<GameObject> AllPrefabs = new List<GameObject>();
    [SerializeField] int _bounds;
    [SerializeField] Transform _golPos;
    [SerializeField] Transform _homePos;
    [SerializeField] float _speed;
    public bool doJob;
    
    void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(
                UnityEngine.Random.Range(-_bounds, _bounds),
                0.5f,
                UnityEngine.Random.Range(-_bounds, _bounds));

            GameObject go = Instantiate(Prefab, pos, Quaternion.identity);
            go.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
            AllPrefabs.Add(go);
        }
    }

    void Update()
    {
        //Change the goal position one out of every 5 times. Hopefully
        if(UnityEngine.Random.Range(0, 20) < 1)
        {
            GoalPosition();
        }
        if(doJob)
        {

        }
        else
        {
            Movement();
        }
    }

    void Movement()
    {
        for(int i = 0; i < AllPrefabs.Count; i++)
        {
            if(Vector3.Distance(AllPrefabs[i].transform.position, _golPos.transform.position) > 1f)
            {
                AllPrefabs[i].transform.position = Vector3.MoveTowards(AllPrefabs[i].transform.position, _golPos.transform.position, UnityEngine.Random.Range(1, 3) * Time.deltaTime);
                Vector3 direction = (_golPos.transform.position - AllPrefabs[i].transform.position).normalized;

                Quaternion _rot = Quaternion.LookRotation(direction);
                AllPrefabs[i].transform.rotation = Quaternion.Slerp(AllPrefabs[i].transform.rotation, _rot, _speed * Time.deltaTime);
                float xRot = AllPrefabs[i].transform.rotation.x; 
                float zRot = AllPrefabs[i].transform.rotation.z;
                
                xRot = Mathf.Clamp(xRot, -10, 10 );
                zRot = Mathf.Clamp(zRot, -1, 1 );
            }
        }
    }

    void GoalPosition()
    {
        Vector3 nextPos = new Vector3
            (
                //change goal position with offset
                UnityEngine.Random.Range(-5, 5),
                1,
                UnityEngine.Random.Range(-5, 5)
            );
        _golPos.position = nextPos;
    }
    // [BurstCompile]
    public struct FlockJob : IJobParallelFor
    {
        public NativeArray<float3> goalPosition;
        public NativeArray<float3> bugPosition;
        public float deltaTime;
        public void Execute(int index) 
        {
            //TO-DO
        }
    }
}
