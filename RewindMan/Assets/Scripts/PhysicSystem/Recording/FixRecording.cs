using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;


public class FixRecording
{
    public class Record
    {
        public FixVec3 velocity;
        public Fix time;
        public FixVec3 position;
        public bool kinematic = false;
        public Record(FixVec3 velocity, Fix time, FixVec3 position, bool kinematic = false)
        {
            this.velocity = velocity;
            this.time = time;
            this.position = position;
            this.kinematic = kinematic;
        }
        public bool Equals(Record other)
        {
            return velocity.Equals(other.velocity) && position.Equals(other.position) && velocity.GetMagnitude() < new Fix(8);
        }
        public override string ToString()
        {
            return velocity.ToString();
        }
    }

    private Stack<Record> records = new Stack<Record>();

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
        while (last != null && last.time > time)
        {
            records.Pop();
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