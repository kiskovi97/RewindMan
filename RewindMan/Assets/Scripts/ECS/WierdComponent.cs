using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WierdComponent : MonoBehaviour
{
    public float speed = 15f;
}

class WierdParticleSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach<WierdComponent, Transform>((Entity entity, WierdComponent p, Transform e) => {
            Debug.Log("In the foreach");
            e.Rotate(0f, p.speed * Time.deltaTime, 0f);
        });
    }
}
