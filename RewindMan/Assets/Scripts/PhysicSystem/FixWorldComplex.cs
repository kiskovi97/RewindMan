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
    private FixPlayerOther fixPlayer;

    private InputRecording stateRecordings = new InputRecording();

    private InputRecord state = new InputRecord();

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
        fixPlayer = FindObjectOfType<FixPlayerOther>();
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

        ReverseCheck();
        if (Forward && !GameOver)
        {
            time += deltaTime;
            state.time = time;
            SimulateForward();
        }
        else if (Backward)
        {
            GameOver = false;
            SimulateBackward();
            time -= deltaTime;
            state.time = time;
        }

        simulate = false;
    }

    private void ReverseCheck()
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
        InputToState();
        Step();
    }

    private void SimulateBackward()
    {
        if (objects.Length == 0) return;

        if (objects[0].CacheSize() == 0)
        {
            ReSimulateFromPoint();
        }
        InputToState(time);
        SetFromCache();
        stateRecordings.ClearFrom(time);
    }

    private void ReSimulateFromPoint()
    {
        Fix fromTime = 0;
        SetState();
        Fix to = time;
        for (time = fromTime; time < to; time += deltaTime)
        {
            RecordToCache();
            InputToState(time + deltaTime);
            state.time = time;
            Step();
        }
    }

    private void InputToState()
    {
        if (Input.GetKey(KeyCode.D)) state.right = true;
        else state.right = false;

        if (Input.GetKey(KeyCode.A)) state.left = true;
        else state.left = false;

        if (Input.GetKey(KeyCode.Space)) state.up = true;
        else state.up = false;

        stateRecordings.AddState(state);
    }

    private void InputToState(Fix fromTime)
    {
        state = stateRecordings.GetState(fromTime);
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
        fixPlayer.KeyCheck(state);
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
