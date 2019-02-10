using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

public class PhysicRecording
{
    public class Record
    {
        public FixVec3 velocity;
        public Fix time;
        public Record(FixVec3 velocity, Fix time)
        {
            this.velocity = velocity;
            this.time = time;
        }
    }

    private Stack<Record> records = new Stack<Record>();

    public void Add(Record record)
    {
        records.Push(record);
    }

    public Record Get(Fix time)
    {
        Record last = records.Peek();
        while (last.time > time)
        {
            records.Pop();
            last = records.Peek();
        }
        if (last.time == time)
        {
            return last;
        }
        return null;
    }
}