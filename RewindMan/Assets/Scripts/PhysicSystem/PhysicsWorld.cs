using UnityEngine;
using System.Collections.Generic;
using FixedPointy;
using System;

public class PhysicsWorld : MonoBehaviour
{
    public static Fix time = Fix.Zero;
    public static FixVec3 gravity = FixVec3.Zero;
    public Vector3 starGravity = new Vector3(0, 0, 0);
    public float timeFloat = 0f;
    public static Fix deltaTime;
    public static volatile bool forward = true;
    public static volatile bool backward = false;
    public void Start()
    {
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        gravity = FixConverter.ToFixVec3(starGravity);
    }
    private void FixedUpdate()
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

        if (forward)
        {
            time += deltaTime;
            PhysicalObject[] objects = FindObjectsOfType<PhysicalObject>();
            MoveAll(objects);
            CollisionDetection(objects);
        }
        else if (backward)
        {
            PhysicalObject[] objects = FindObjectsOfType<PhysicalObject>();
            CollisionDetectionBackWard(objects);
            MoveAll(objects);
            time -= deltaTime;
        }
        timeFloat = (float)time;
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
        for (int i= objects.Length - 1; i>= 0; i--)
        {
            List<PhysicalObject> collided = new List<PhysicalObject>();
            for (int j = objects.Length - 1; j >= 0; j--)
            {
                if (i == j) continue;
                if (objects[i].IsCollided(objects[j])) collided.Add(objects[j]);
            }
            objects[i].CollideAll(collided.ToArray());
        }
    }

    private void CollisionDetectionBackWard(PhysicalObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].CollideBack();
        }
    }
}
