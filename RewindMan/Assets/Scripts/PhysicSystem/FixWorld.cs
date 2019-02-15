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

    // Inner state
    private static volatile bool forward = true;
    private static volatile bool backward = false;
    private FixObject[] objects;
    private FixPlayer player;

    public void Awake()
    {
        gravity = FixConverter.ToFixVec3(Physics.gravity);
    }

    public void Start()
    {
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        objects = FindObjectsOfType<FixObject>();
        player = FindObjectOfType<FixPlayer>();
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

    private void Update()
    {
        if (forward)
        {
            player.KeyCheck();
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
            List<FixObject> collided = new List<FixObject>();
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
