using UnityEngine;
using System.Collections;
using FixedPointy;
using System;
namespace FixPhysics
{
    public class Record<T> where T : ICloneable
    {
        public T state;
        public Fix time;
        public bool kinematic = false;
        public Record(T state, Fix time, bool kinematic = false)
        {
            this.state = (T)state.Clone();
            this.time = time;
            this.kinematic = kinematic;
        }

        public bool Equals(Record<T> other)
        {
            return state.Equals(other.state);
        }

        public override string ToString()
        {
            return state.ToString();
        }

        public virtual Record<T> Copy()
        {
            return new Record<T>(this);
        }

        protected Record(Record<T> record)
        {
            this.state = (T)record.state.Clone();
            this.time = record.time;
            this.kinematic = record.kinematic;
        }
    }
}
