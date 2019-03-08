using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class FixWorldComplex : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static Fix time = Fix.Zero;
    public static Fix deltaTime;
    public string timeOut = "";

    private RecordedObjectOther[] objects;

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

    public void Awake()
    {
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        time = Fix.Zero;
    }

    public void Start()
    {
        GameOver = false;
        Forward = true;
        Backward = false;
        objects = FindObjectsOfType<RecordedObjectOther>();
        Record();
    }

    private void FixedUpdate()
    {
        InputCheck();
        if (Forward && !GameOver)
        {
            SimulateForward();
            time += deltaTime;
        }
        else if (Backward)
        {
            GameOver = false;
            time -= deltaTime;
            SimulateBackward();
        }
        timeOut = recordsTime.Count + " " + lastRecord;
    }

    private void InputCheck()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (Forward)
            {
                CacheClear();
                ReSimulateFromPoint();
            }
            Forward = false;
            if (time > Fix.Zero) Backward = true;
            else Backward = false;
        }
        else
        {
            if (Backward)
            {
                Record();
            }
            Forward = true;
            Backward = false;
        }
    }

    private int framePerRecord = 60;

    private int lastRecord = 60;

    private Stack<int> records = new Stack<int>();
    private Stack<Fix> recordsTime = new Stack<Fix>();

    private void SimulateForward()
    {
        CacheClear();
        Step();
        lastRecord++;
        if (framePerRecord <= lastRecord)
        {
            lastRecord = 0;
            Record();
        }
    }

    private void SimulateBackward()
    {
        if (lastRecord <= 0)
        {
            ReSimulateFromPoint();
            lastRecord = framePerRecord - 1;
        }
        else
        {
            SetFromCache();
            lastRecord--;
        }
    }

    private void ReSimulateFromPoint()
    {
        Fix fromTime = recordsTime.Pop();
        if (recordsTime.Count == 0) recordsTime.Push(fromTime);
        SetState();
        for (Fix i = fromTime; i < time; i += deltaTime)
        {
            Step();
            RecordToCache();
        }
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
            objects[i].Step();
        }
    }

    private void Record()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Record();
        }
        recordsTime.Push(time);
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
