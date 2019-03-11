using UnityEngine;
using FixedPointy;
using System.Collections.Generic;

public class RecordedObjectOther : MonoBehaviour
{
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public Record state;

    public int recordsNumber = 0;

    Stack<Record> cache = new Stack<Record>();
    Stack<Record> recording = new Stack<Record>();

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
        if (newVelocity.GetMagnitude() < FixWorldComplex.deltaTime * FixWorldComplex.gravity.GetMagnitude()) newVelocity = FixVec3.Zero;
    }

    protected void Accelerate(FixVec3 sumForce)
    {
        state.velocity += sumForce * FixWorldComplex.deltaTime;
    }

    public void Step()
    {
        state.position += state.velocity * FixWorldComplex.deltaTime;
    }

    public void Record()
    {
        state.time = FixWorldComplex.time;
        recording.Push(state.Copy());
    }

    public void SetLast()
    {
        Record record = recording.Pop();
        if (record != null)
        {
            state = record.Copy();
        }
        if (recording.Count == 0) recording.Push(record);
    }

    public void RecordToCache(Fix time)
    {
        if (cache.Count > 0)
        {
            Record rec = cache.Peek();
            Debug.DrawLine(FixConverter.ToFixVec3(rec.position), FixConverter.ToFixVec3(state.position), Color.red, (float)time);
        }
        state.time = FixWorldComplex.time;
        cache.Push(state.Copy());
    }

    public void CacheClear()
    {
        cache.Clear();
    }

    public int CacheSize()
    {
        return cache.Count;
    }

    public void SetFromCache()
    {
        Record record = cache.Pop();
        state = record.Copy();
    }
}
