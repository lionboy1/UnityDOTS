using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///Attach this to a regular Game Object(Non-ECS) to act as the target to be followed
public class GoalChanger : MonoBehaviour
{
    [SerializeField]
    Transform _golPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Call GoalPostion everytime 0 is selected.  Adjust for frequency
        if(UnityEngine.Random.Range(0, 20) < 1)
        {
            GoalPosition();
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
}
