using System;
using System.Collections.Generic;
using FixedPointy;
namespace FixPhysics
{
    public class StateRecording<T> where T : ICloneable
    {
        Stack<Record<T>> records = new Stack<Record<T>>();
        public void Push(T record)
        {
            records.Push(new Record<T>(record, FixWorld.time));
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
        public T GetByTime(Fix time)
        {
            Record<T> record = records.Pop();
            while (record.time > time)
            {
                if (records.Count == 0) break;
                record = records.Pop();
            }
            if (record.time <= time)
            {
                records.Push(record);
            } else if (records.Count == 0) records.Push(record);
            return (T)record.state.Clone();
        }
    }
}
