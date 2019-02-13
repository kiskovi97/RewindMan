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
        public FixVec3 position;
        public Record(FixVec3 velocity, Fix time, FixVec3 position)
        {
            this.velocity = velocity;
            this.time = time;
            this.position = position;
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