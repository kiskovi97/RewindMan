using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;


public class FixRecordingOther
{
    private Stack<Record> records = new Stack<Record>();

    public void Reset()
    {
        records = new Stack<Record>();
    }

    public int RecordNumber()
    {
        return records.Count;
    }

    private Fix prevTime = 0;

    public void Add(FixVec3 velocity, Fix time, FixVec3 position, bool Draw = false)
    {
        Record record = new Record(velocity, time, position);
        records.Push(record);
    }

    public Record Get(Fix time)
    {
        if (records.Count == 0) return null;
        Record last = records.Pop();
        Record prev = records.Peek();
        while (last.time > time)
        {
            last = records.Pop();
            prev = records.Peek();
        }
        if (prev == null)
        {
            records.Push(last);
        }
        return last;
    }
}