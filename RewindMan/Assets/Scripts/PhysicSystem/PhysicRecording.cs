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

    public void Add(FixVec3 velocity, Fix time, FixVec3 position)
    {
        Record record = new Record(velocity, time, position);
        records.Push(record);
    }

    public void AddForceChange(FixVec3 force, Fix time)
    {
        ForceRecord record = new ForceRecord(force, time);
        forceRecords.Push(record);
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
        if (output.time == time)
        {
            return output;
        }
        return null;
    }

    public ForceRecord GetForceChange(Fix time)
    {
        if (forceRecords.Count == 0) return null;
        ForceRecord last = forceRecords.Peek();
        while (last.time > time)
        {
            forceRecords.Pop();
            if (forceRecords.Count == 0) return null;
            last = forceRecords.Peek();
        }
        if (last.time == time)
        {
            return last;
        }
        return null;
    }
}