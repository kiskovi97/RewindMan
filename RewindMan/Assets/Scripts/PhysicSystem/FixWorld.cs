using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using FixedPointy;
using System;

public class FixWorld : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static Fix time = Fix.Zero;
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix deltaTime;

    public float timeOut = 0f;

    public static bool GameOver
    {
        get; private set;
    }

    public static bool Forward
    {
        get; private set;
    }

    public static bool Backward
    {
        get; private set;
    }

    // Inner state
    private FixObject[] objects;
    private FixPlayer player;

    public void Awake()
    {
        gravity = FixConverter.ToFixVec3(Physics.gravity);
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        time = Fix.Zero;
    }

    public void Start()
    {
        List<FixObject> list = new List<FixObject>();
        list.AddRange(FindObjectsOfType<RigidObject>());
        list.AddRange(FindObjectsOfType<MovingPlatform>());
        objects = list.ToArray();
        player = FindObjectOfType<FixPlayer>();
        GameOver = false;
    }



    public static void GameOverSet()
    {
        GameOver = true;
    }

    public static FixVec3 GravitySizeVector(FixVec3 vector)
    {
        return vector.Normalize() * gravity.GetMagnitude();
    }

    private void FixedUpdate()
    {
        InputCheck();
        if (Forward && !GameOver)
        {
            MoveAll();
            time += deltaTime;
        }
        else if (Backward)
        {
            GameOver = false;
            time -= deltaTime;
            MoveAllBack();
        }
        timeOut = (float)time;
    }

    private void Update()
    {
        if (Forward)
        {
            player.KeyCheck();
        }
    }

    private void InputCheck()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Forward = false;
            if (time > Fix.Zero) Backward = true;
            else Backward = false;
        }
        else
        {
            Forward = true;
            Backward = false;
        }
    }

    private void MoveAll()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Move();

            if (!objects[i].IsStatic())
            {
                List<Collision> collisions = new List<Collision>();
                for (int j = objects.Length - 1; j >= 0; j--)
                {
                    if (i == j) continue;
                    Collision collision = objects[j].GetCollision(objects[i]);
                    if (collision != null)
                        collisions.Add(collision);
                }
                objects[i].Collide(collisions.ToArray());
            }
        }
    }

    private void MoveAllBack()
    {
        for (int i = objects.Length - 1; i >= 0; i--)
        {
            if (!objects[i].IsStatic())
            {
                List<Collision> collisions = new List<Collision>();
                for (int j = objects.Length - 1; j >= 0; j--)
                {
                    if (i == j) continue;
                    Collision collision = objects[j].GetCollision(objects[i]);
                    if (collision != null)
                        collisions.Add(collision);
                }
                objects[i].CollideBack(collisions.ToArray());
            }
            

            objects[i].MoveBackwards();
        }
    }
}
