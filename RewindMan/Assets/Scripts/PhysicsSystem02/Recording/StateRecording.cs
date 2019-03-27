using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

public class StateRecording
{

    Stack<Record> records = new Stack<Record>();

    public void Push(Record record)
    {
        records.Push(record);
    }

    public int Count
    {
        get
        {
            return records.Count;
        }
    }

    public void Clear()
    {
        records.Clear();
    }

    public Record GetByTime(Fix time)
    {
        Record record = records.Pop();
        while (record.time > time)
        {
            if (records.Count == 0) break;
            record = records.Pop();
        }
        if (record.time <= time)
        {
            records.Push(record);
        } else if (records.Count == 0) records.Push(record);
        return record;
    }
}
