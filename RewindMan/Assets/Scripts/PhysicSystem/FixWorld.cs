using UnityEngine;
using System.Collections.Generic;
using FixedPointy;
using System;

public class FixWorld : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static Fix time = Fix.Zero;
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix deltaTime;

    // Initial values
    public Vector3 starGravity = new Vector3(0, 0, 0);

    // Inner state
    private static volatile bool forward = true;
    private static volatile bool backward = false;
    private PhysicalObject[] objects;

    public void Start()
    {
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        gravity = FixConverter.ToFixVec3(starGravity);
        objects = FindObjectsOfType<PhysicalObject>();
    }

    public static FixVec3 GravitySizeVector(FixVec3 vector)
    {
        return vector.Normalize() * gravity.GetMagnitude();
    }

    private void FixedUpdate()
    {
        InputCheck();
        if (forward)
        {
            time += deltaTime;
            MoveAll();
            CollisionDetection();
        }
        else if (backward)
        {
            CollisionDetectionBackWard();
            MoveAllBack();
            time -= deltaTime;
        }
    }

    private void InputCheck()
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

    private void MoveAll()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Move();
        }
    }

    private void MoveAllBack()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].MoveBackwards();
        }
    }

    private void CollisionDetection()
    {
        for (int i= objects.Length - 1; i>= 0; i--)
        {
            List<PhysicalObject> collided = new List<PhysicalObject>();
            for (int j = objects.Length - 1; j >= 0; j--)
            {
                if (i == j) continue;
                if (objects[i].IsCollided(objects[j])) collided.Add(objects[j]);
            }
            objects[i].Collide(collided.ToArray());
        }
    }

    private void CollisionDetectionBackWard()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].CollideBack();
        }
    }
}
