using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class FixWorldComplex : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix time = Fix.Zero;
    public static Fix deltaTime;
    public string timeOut = "";
    private FixCollider[] colliders;

    private InputRecording stateRecordings = new InputRecording();

    private RigidObjectOther[] objects;

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

    public static void GameOverSet()
    {
        GameOver = true;
    }

    public static FixVec3 GravitySizeVector(FixVec3 vector)
    {
        return vector.Normalize() * gravity.GetMagnitude();
    }

    public void Awake()
    {
        gravity = FixConverter.ToFixVec3(Physics.gravity);
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        time = Fix.Zero;
    }

    public void Start()
    {
        GameOver = false;
        Forward = true;
        Backward = false;
        objects = FindObjectsOfType<RigidObjectOther>();
        colliders = FindObjectsOfType<FixCollider>();
    }

    private bool firstTime = true;
    
    private volatile bool simulate = false;

    private void FixedUpdate()
    {
        timeOut = time + "";
        if (simulate) return;
        simulate = true;
        if (firstTime)
        {
            Record();
            firstTime = false;
        }
        InputCheck();
        if (Forward && !GameOver)
        {
            time += deltaTime;
            SimulateForward();
        }
        else if (Backward)
        {
            GameOver = false;
            SimulateBackward();
            time -= deltaTime;
        }
        simulate = false;
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

    private void SimulateForward()
    {
        CacheClear();
        Step();
    }

    private void SimulateBackward()
    {
        if (objects.Length == 0) return;

        if (objects[0].CacheSize() == 0)
        {
            ReSimulateFromPoint();
        }
        SetFromCache();
    }

    private void ReSimulateFromPoint()
    {
        Fix fromTime = 0;
        SetState();
        Fix to = time;
        for (time = fromTime; time < to; time += deltaTime)
        {
            RecordToCache();
            Step();
        }
        stateRecordings.ClearFrom(to);
    }

    private void SetFromCache()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetFromCache();
        }
    }

    private void CacheClear()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].CacheClear();
        }
    }

    private void Step()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Move();
        }
        for (int i = 0; i < objects.Length; i++)
        {
            List<Collision> collisions = new List<Collision>();
            for (int j = colliders.Length - 1; j >= 0; j--)
            {
                Collision collision = objects[i].GetCollision(colliders[j]);
                if (collision != null)
                    collisions.Add(collision);
            }
            objects[i].Collide(collisions.ToArray());
        }
    }

    private void Record()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Record();
        }
    }

    private void RecordToCache()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].RecordToCache();
        }
    }

    private void SetState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetLast();
        }
    }
}
