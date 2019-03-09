using UnityEngine;
using System.Collections.Generic;
using FixedPointy;


[RequireComponent(typeof(FixObjects))]
public class FixWorldComplex : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix time = Fix.Zero;
    public static Fix deltaTime;
    public string timeOut = "";
    private FixObjects fixObjects;

    private InputRecording stateRecordings = new InputRecording();

    private InputRecord state = new InputRecord();

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
        fixObjects = GetComponent<FixObjects>();
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
            fixObjects.Record();
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
        fixObjects.CacheClear();
        InputToState();
        fixObjects.Step(state);
    }

    private void SimulateBackward()
    {
        if (fixObjects.CahceIsEmpty())
        {
            ReSimulateFromPoint();
        }
        InputToState(time);
        fixObjects.SetFromCache();
        stateRecordings.ClearFrom(time);
    }

    private void ReSimulateFromPoint()
    {
        Fix fromTime = 0;
        fixObjects.SetState();
        Fix to = time;
        for (time = fromTime; time < to; time += deltaTime)
        {
            fixObjects.RecordToCache(to - time);
            InputToState(time + deltaTime);
            state.time = time;
            fixObjects.Step(state);
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
}
