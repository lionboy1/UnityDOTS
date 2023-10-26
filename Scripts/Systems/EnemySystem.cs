using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

public class EnemySystem : SystemBase
{
    Unity.Mathematics.Random rand = new Random(1234);//start with a random seed value to select a random direction
 

    protected override void OnUpdate()
    {
        //need to initialize raycaster.  
        var raycaster = new MovementRaycast()
        {
            //access the physics world to do something with the raycast
            phyz = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld
        };

        //Increment so the next position will always be different
        rand.NextInt();
        //Store new next random value
        var nextRandom = rand;

        Entities.ForEach((ref Movement mov, ref Enemy enemy, ref Translation translation, in Rotation rotation) => 
        {
            if(math.distance(translation.Value, enemy.previousDestination) > 0.9f)
            {
                enemy.previousDestination = math.round(translation.Value);

                var rayList = new NativeList<float3>(Allocator.Temp);
                
                if(!raycaster.CastTheRay(translation.Value, new float3(0,0,-1), mov.direction))
                {
                    rayList.Add(new float3(0,0,-1));
                }
                if(!raycaster.CastTheRay(translation.Value, new float3(0,0,1), mov.direction))
                {
                    rayList.Add(new float3(0,0,1));
                }
                if(!raycaster.CastTheRay(translation.Value, new float3(-1,0,0), mov.direction))
                {
                    rayList.Add(new float3(-1,0,0));
                }
                if(!raycaster.CastTheRay(translation.Value, new float3(1,0,0), mov.direction))
                {
                    rayList.Add(new float3(1,0,0));
                }
                rayList.Dispose();
            }

        }).Schedule();
    }

    struct MovementRaycast
    {
        [ReadOnly]//Prevents the physics world from enduring race conditions.
        public PhysicsWorld phyz;//does the actual casting
        public bool CastTheRay(float3 position, float3 direction, float3 currentDirection)
        {
            
            if(direction.Equals(-currentDirection))
            {
                return true;//forces the AI to move forward and not back track the last waypoint
            }
            //Initialize the ray
            var ray = new RaycastInput()
            {
                Start = position,
                End = direction,
                Filter = new CollisionFilter()
                {
                    BelongsTo =  1u << 1, //set the layer the entity belongs to (layer 1). Uint type is used instead of int
                    CollidesWith = 1u << 2, //Layers to collide against. Will collide with walls on layer 2
                    GroupIndex = 0//Not important for now
                }
            };
            
            return phyz.CastRay(ray);
        }
    }
}
