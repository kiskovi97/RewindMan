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
        public Record(FixVec3 velocity, Fix time, FixVec3 position)
        {
            this.velocity = velocity;
            this.time = time;
            this.position = position;
        }
        public bool Equals(Record other)
        {
            return velocity.Equals(other.velocity) && position.Equals(other.position);
        }
    }

    public class ForceRecord
    {
        public FixVec3 force;
        public Fix time;
        public ForceRecord(FixVec3 force, Fix time)
        {
            this.force = force;
            this.time = time;
        }
    }

    private Stack<Record> records = new Stack<Record>();
    private Stack<ForceRecord> forceRecords = new Stack<ForceRecord>();

    public int RecordNumber()
    {
        return records.Count;
    }

    public void Add(FixVec3 velocity, Fix time, FixVec3 position, bool Draw = false)
    {
        Record record = new Record(velocity, time, position);
        if (records.Count != 0)
        {
            if (records.Peek().time != record.time)
            {
                if (!record.Equals(records.Peek()))
                {
                    records.Push(record);
                }
                else
                {
                    Record rec = records.Peek();
                    rec.time = time;
                    rec.kinematic = true;
                }
            }
        }
        else records.Push(record);
    }

    public Record Get(Fix time)
    {
        Record last = records.Peek();
        while (last.time > time)
        {
            records.Pop();
            last = records.Peek();
        }
        Record output = last;
        while (last != null && last.time == time)
        {
            output = records.Pop();
            last = records.Peek();
        }
        if (last == null) records.Push(output);
        if (output.kinematic)
        {
            output.time = time - FixWorld.deltaTime;
            records.Push(output);
            return output;
        }
        if (output.time == time)
        {
            return output;
        }
        return null;
    }
}