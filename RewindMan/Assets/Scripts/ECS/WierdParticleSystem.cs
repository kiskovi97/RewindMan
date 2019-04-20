using UnityEngine;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Assets.Scripts.ECS
{
    [UpdateAfter(typeof(FixedUpdate))]
    class WierdParticleSystem : ComponentSystem
    {
        float prevTime = 0;

        protected override void OnUpdate()
        {
            float delta = Time.time - prevTime;
            Debug.Log("UpdateAfter: " + delta);
            Entities.ForEach<WierdComponent, Transform>((Entity entity, WierdComponent p, Transform e) => {
                e.Translate(Time.fixedDeltaTime, 0f, 0f);
            });
            prevTime = Time.time;
        }
    }
    
    [UpdateBefore(typeof(FixedUpdate))]
    class WierdParticleSystem2 : ComponentSystem
    {
        float prevTime = 0;

        protected override void OnUpdate()
        {
            float delta = Time.time - prevTime;
            Debug.Log("UpdateBefore: " + delta);
            Entities.ForEach<WierdComponent, Transform>((Entity entity, WierdComponent p, Transform e) => {
                e.Translate(Time.fixedDeltaTime, 0f, 0f);
            });
            prevTime = Time.time;
        }
    }
}
