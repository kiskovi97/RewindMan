using UnityEngine;
using System.Collections.Generic;
using FixedPointy;
using System;

public class FixWorld : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public Light light;
    public GlitchEffect effect;
    public float glitchIntensity = 0.2f;
    public Color reverseColor;
    private Color prevColor;
    public static Fix time = Fix.Zero;
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix deltaTime;

    public float timeOut = 0f;

    // Inner state
    private static volatile bool forward = true;
    private static volatile bool backward = false;
    private static volatile bool gameOver = false;
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
        prevColor = light.color;
        gameOver = false;
    }

    public static bool IsGameOver
    {
        get { return gameOver; }
    }

    public static void GameOver()
    {
        gameOver = true;
    }

    public static FixVec3 GravitySizeVector(FixVec3 vector)
    {
        return vector.Normalize() * gravity.GetMagnitude();
    }

    private void FixedUpdate()
    {
        InputCheck();
        if (gameOver)
            SetBackWardEffect();
        if (forward && !gameOver)
        {
            SetForwardEffect();
            MoveAll();
            CollisionDetection();
            time += deltaTime;
        }
        else if (backward)
        {
            gameOver = false;
            SetBackWardEffect();
            time -= deltaTime;
            CollisionDetectionBackWard();
            MoveAllBack();
        }
        timeOut = (float)time;
    }

    private void SetForwardEffect()
    {
        if (light != null)
        {
            light.color = prevColor;
        }
        if (effect != null)
        {
            effect.intensity = 0;
            effect.colorIntensity = 0;
        }
    }

    private void SetBackWardEffect()
    {
        if (light != null)
        {
            light.color = reverseColor;
        }
        if (effect != null)
        {
            effect.intensity = glitchIntensity;
            effect.colorIntensity = 1f;
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
        for (int i = objects.Length - 1; i >= 0; i--)
        {
            if (objects[i].IsStatic()) continue;
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

    private void CollisionDetectionBackWard()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].IsStatic()) continue; if (objects[i].IsStatic()) continue;
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
    }
}
