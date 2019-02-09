using UnityEngine;
using System.Collections;
using FixedPointy;
using System;

public class PhysicsWorld : MonoBehaviour
{
    public static Fix time = Fix.Zero;
    public float timeFloat = 0f;
    public static Fix deltaTime;
    public static volatile bool forward = true;
    public static volatile bool backward = false;
    public void Start()
    {
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
    }
    private void FixedUpdate()
    {
        if (forward)
        {
            time += deltaTime;
        }
        else if (backward)
        {
            time -= deltaTime;
        }
        timeFloat = (float)time;
        PhysicalObject[] objects = FindObjectsOfType<PhysicalObject>();
        MoveAll(objects);
        CollisionDetection(objects);
    }

    private void MoveAll(PhysicalObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Move();
        }
    }

    private void CollisionDetection(PhysicalObject[] objects)
    {
        for (int i=0; i< objects.Length; i++)
        {
            for (int j=0; j< objects.Length; j++)
            {
                if (i == j) continue;
                objects[i].collider.Collide(objects[j]);
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            forward = false;
            if (time > Fix.Zero) backward = true;
            else backward = false;
        }
        else
        {
            forward = true;
            backward = false;
        }
    }
}
