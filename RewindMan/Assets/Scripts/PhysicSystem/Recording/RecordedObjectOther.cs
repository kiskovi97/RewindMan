using UnityEngine;
using FixedPointy;
using System.Collections.Generic;

class RecordedObjectOther : MonoBehaviour
{
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public FixVec3 Position { get; private set; }

    public FixVec3 Velocity { get; private set; }

    public int recordsNumber = 0;

    Stack<Record> cache = new Stack<Record>();
    Stack<Record> recording = new Stack<Record>();

    protected virtual void Start()
    {
        Position = FixConverter.ToFixVec3(transform.position);
        Velocity = FixConverter.ToFixVec3(startVelocity);
    }

    private void Update()
    {
        recordsNumber = recording.Count;
        transform.position = FixConverter.ToFixVec3(Position);
        startVelocity = FixConverter.ToFixVec3(Velocity);
    }

    protected void ResetRecording()
    {
        recording.Clear();
    }

    protected void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    protected void PositionCorrection(FixVec3 newPosition)
    {
        Position = newPosition;
    }

    protected void VelocityCorrection(FixVec3 newVelocity)
    {
        Velocity = newVelocity;
        if (newVelocity.GetMagnitude() < FixWorldComplex.deltaTime * FixWorldComplex.gravity.GetMagnitude()) newVelocity = FixVec3.Zero;
    }

    protected void Accelerate(FixVec3 sumForce)
    {
        Velocity += sumForce * FixWorldComplex.deltaTime;
    }

    public void Step()
    {
        Position += Velocity * FixWorldComplex.deltaTime;
    }

    public void Record()
    {
        recording.Push(new Record(Velocity, FixWorldComplex.time, Position));
    }

    public void SetLast()
    {
        Record record = recording.Pop();
        if (record != null)
        {
            Velocity = record.velocity;
            Position = record.position;
        }
        if (recording.Count == 0) recording.Push(record);
    }

    public void RecordToCache()
    {
        if (cache.Count > 0)
        {
            Record rec = cache.Peek();
            Debug.DrawLine(FixConverter.ToFixVec3(rec.position), FixConverter.ToFixVec3(Position), Color.red, 1000f);
        }
        cache.Push(new Record(Velocity, FixWorldComplex.time, Position));
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
        Velocity = record.velocity;
        Position = record.position;
        Debug.Log(record.time);
    }
}
