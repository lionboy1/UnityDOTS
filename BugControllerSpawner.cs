using UnityEngine;


public class BugControllerSpawner : MonoBehaviour
{
    [SerializeField] int spawnAmount;
    public GameObject Prefab;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;

    void Start()
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            GameObject go = Instantiate(Prefab, new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), -17, UnityEngine.Random.Range(-2.5f, 3.5f)), Quaternion.AngleAxis(90, Vector3.up ));
            GameObject go2 = Instantiate(Prefab2, new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), -17, UnityEngine.Random.Range(-2.5f, 3.5f)), Quaternion.AngleAxis(90, Vector3.up ));
            GameObject go3 = Instantiate(Prefab3, new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), -17, UnityEngine.Random.Range(-2.5f, 3.5f)), Quaternion.AngleAxis(90, Vector3.up ));
            GameObject go4 = Instantiate(Prefab4, new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), -17, UnityEngine.Random.Range(-2.5f, 3.5f)), Quaternion.AngleAxis(90, Vector3.up ));
        }
    }
}