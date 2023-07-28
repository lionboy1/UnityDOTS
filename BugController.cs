using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

public struct BugsOnScreenJob : IJob
{
    public void Execute()
    {

    }
}

public class BugController : MonoBehaviour
{
    // [SerializeField] float _xMove;
    // [SerializeField] float _yMove;
    Vector2 _screenBounds;
    float objWidth;
    float objHeight;
    GameObject go;
    [SerializeField] Camera _cam;
    [SerializeField] float moveSpeed;

    // void Awake()
    // {
    //     _cam = Camera.main;
    //     _screenBounds = _cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _cam.transform.position.z));
        
    // }
    void Start()
    {
        // go = Instantiate(prefab, new Vector3(0, -17, 0), Quaternion.AngleAxis(90, Vector3.up));
        //Get the size of object to offset. Prevents it from clipping through the border
        // objWidth = go.transform.GetComponent<Collider>().bounds.size.x;
        // objHeight = go.transform.GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        // Vector3 viewPosition = transform.position;
        // //Clamp X movement coordinates to X boundaries
        // viewPosition.x = Mathf.Clamp(viewPosition.x, _screenBounds.x , _screenBounds.x + 1);
        // viewPosition.y = Mathf.Clamp(viewPosition.y, _screenBounds.y , _screenBounds.y + 1);
        // //update the transform position to the viewport
        // transform.position = viewPosition;
        Movement();
    }
    void Movement()
    {
        
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0 );
        
        if(transform.position.x >= 10)
        {
            transform.Rotate(0, -180, 0);
            moveSpeed = -moveSpeed;
        }
        if(transform.position.x <= -10)
        {
            transform.Rotate(0, 180, 0);
            moveSpeed = math.abs(moveSpeed);
        }
    }
}