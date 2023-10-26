using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;

public class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        
        Entities.ForEach((ref PhysicsVelocity pv, ref Movement mov) => 
        {
            mov.direction = new float3(x, 0, z); //feed arrow keys input to the 'Movement' component x, y, z values
            var step = mov.direction * mov.speed;// each step is a change in direction based on speed
            pv.Linear = step;//assign the step values to the Linear field of the PhysicsVelocity struct to move
            
        }).Schedule();
    }
}
