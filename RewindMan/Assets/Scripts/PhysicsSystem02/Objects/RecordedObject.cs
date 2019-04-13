using UnityEngine;
using FixedPointy;
using System.Collections.Generic;
using System;
namespace FixPhysics
{
    public abstract class RecordedObject<T> : MonoBehaviour, IRecordedObject where T : ICloneable
    {
        public bool TimeResistance = false;
        public T state;
        private T savedState;

        public int recordsNumber = 0;

        Stack<Record<T>> cache = new Stack<Record<T>>();
        StateRecording<T> recording = new StateRecording<T>();

        public virtual void Update()
        {
            recordsNumber = recording.Count;
        }

        public void ResetRecording()
        {
            recording.Clear();
        }

        public void Record()
        {
            recording.Push(state);
        }

        public void SetLast(Fix time)
        {
            savedState = (T)state.Clone();
            T record = recording.GetByTime(time);
            if (record != null)
            {
                state = record;
            }
        }

        public void RecordToCache(Fix time)
        {
            if (TimeResistance) return;
            cache.Push(new Record<T>(state, FixWorld.time));
        }

        public void CacheClear()
        {
            cache.Clear();
        }

        public int CacheSize()
        {
            if (TimeResistance) return -1;
            return cache.Count;
        }

        public void SetFromCache()
        {
            if (TimeResistance)
            {
                state = savedState;
                return;
            }
            Record<T> record = cache.Pop();
            state = (T)record.state.Clone();
        }
    }
}
