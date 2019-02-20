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
            return velocity + " " + time + " " + position;
        }
    }

    private Stack<Record> records = new Stack<Record>();

    public int RecordNumber()
    {
        return records.Count;
    }

    public void Add(FixVec3 velocity, Fix time, FixVec3 position, bool Draw = false)
    {
        Record record = new Record(velocity, time, position);
        if (records.Count == 0) records.Push(record);
        else
        {
            Record prev = records.Peek();

            if (prev.time == time) return;

            if (prev.Equals(record))
            {
                record.kinematic = true;
                if (prev.kinematic == true) records.Pop();
            }

            records.Push(record);
        }
    }

    public Record Get(Fix time)
    {
        Record last = records.Peek();
        while (last != null && last.time > time)
        {
            Record prev = records.Pop();
            last = records.Peek();
            if (prev.kinematic && prev.time > (last.time + FixWorld.deltaTime))
            {
                prev.time -= FixWorld.deltaTime;
                last = prev;
                records.Push(last);
            }
        }
        //  last.time <= time
        if (last.time == time)
        {
            return last;
        }
        return null;
    }
}