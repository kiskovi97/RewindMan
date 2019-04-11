using UnityEngine;
using FixedPointy;
using System.Collections.Generic;
namespace FixPhysics
{
    public abstract class MovingObject : MonoBehaviour
    {
        public bool TimeResistance = false;
        public Vector3 startVelocity = new Vector3(0, 1, 0);
        public Record state;
        private Record savedState;

        public int recordsNumber = 0;

        Stack<Record> cache = new Stack<Record>();
        StateRecording recording = new StateRecording();

        protected virtual void Start()
        {
            FixVec3 Position = FixConverter.ToFixVec3(transform.position);
            FixVec3 Velocity = FixConverter.ToFixVec3(startVelocity);
            state = new Record(Velocity, 0, Position);
        }

        private void Update()
        {
            recordsNumber = recording.Count;
            transform.position = FixConverter.ToFixVec3(state.position);
            startVelocity = FixConverter.ToFixVec3(state.velocity);
        }

        protected void ResetRecording()
        {
            recording.Clear();
        }

        protected void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
        {
            state.position = position;
            state.velocity = velocity;
        }

        protected void PositionCorrection(FixVec3 newPosition)
        {
            state.position = newPosition;
        }

        protected void VelocityCorrection(FixVec3 newVelocity)
        {
            state.velocity = newVelocity;
            if (newVelocity.GetMagnitude() < FixWorld.deltaTime * FixWorld.gravity.GetMagnitude()) newVelocity = FixVec3.Zero;
        }

        protected void Accelerate(FixVec3 sumForce)
        {
            state.velocity += sumForce * FixWorld.deltaTime;
        }

        public abstract void Move();

        public void Step()
        {
            state.position += state.velocity * FixWorld.deltaTime;
        }

        public void Step(FixVec3 velocity)
        {
            state.position += velocity * FixWorld.deltaTime;
        }

        public void Record()
        {
            state.time = FixWorld.time;
            recording.Push(state.Copy());
        }

        public void SetLast(Fix time)
        {
            savedState = state.Copy();
            Record record = recording.GetByTime(time);
            if (record != null)
            {
                state = record.Copy();
            }
        }

        public void RecordToCache(Fix time)
        {
            if (TimeResistance) return;
            if (cache.Count > 0)
            {
                Record rec = cache.Peek();
                Debug.DrawLine(FixConverter.ToFixVec3(rec.position), FixConverter.ToFixVec3(state.position), Color.red, (float)time);
            }
            state.time = FixWorld.time;
            cache.Push(state.Copy());
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
            Record record = cache.Pop();
            state = record.Copy();
        }
    }
}
