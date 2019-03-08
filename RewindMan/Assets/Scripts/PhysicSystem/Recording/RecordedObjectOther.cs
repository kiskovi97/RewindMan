using UnityEngine;
using FixedPointy;
using System.Collections.Generic;

class RecordedObjectOther : MonoBehaviour
{
    public FixVec3 Position { get; private set; }

    public FixVec3 Velocity { get; private set; }

    public int recordsNumber = 0;

    Stack<Record> cache = new Stack<Record>();
    Stack<Record> recording = new Stack<Record>();

    void Start()
    {
        Position = FixConverter.ToFixVec3(transform.position);
        Velocity = FixConverter.ToFixVec3(new Vector3(0,1,0));
    }

    private void Update()
    {
        recordsNumber = recording.Count;
        transform.position = FixConverter.ToFixVec3(Position);
    }

    protected void ResetRecording()
    {
        recording.Clear();
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
        cache.Push(new Record(Velocity, FixWorldComplex.time, Position));
    }

    public void CacheClear()
    {
        cache.Clear();
    }

    public void SetFromCache()
    {
        Record record = cache.Pop();
        Velocity = record.velocity;
        Position = record.position;
    }
}
