using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;


public class FixRecording
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
        if (records.Count == 0) records.Push(record);
        else
        {
            Record prev = records.Peek();
            if (prevTime != time)
            {
                if (!prev.Equals(record))
                {
                    records.Push(record);
                }
                else
                {
                    if (!prev.kinematic)
                    {
                        record.kinematic = true;
                        records.Push(record);
                    }
                }
            }
        }
        prevTime = time;
    }

    public Record Get(Fix time)
    {
        prevTime = time;
        Record last = records.Peek();
        Record prev = records.Peek();
        while (last != null && last.time > time)
        {
            prev = records.Pop();
            if (records.Count == 0)
            {
                records.Push(prev);
                if (prev.time >= time)
                    return prev;
                else
                    return null;
            }
            last = records.Peek();
        }
        //  last.time <= time
        if (last.time == time)
        {
            return last;
        } else
        {
            if (last.kinematic) return last;
        }
        return null;
    }
}