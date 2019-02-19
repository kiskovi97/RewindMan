using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

class RecordedObject : MonoBehaviour
{
    private FixRecording recording = new FixRecording();

    public FixVec3 Position { get; private set; }

    public FixVec3 Velocity { get; private set; }

    public bool Kinematic { get; private set; }

    public bool DrawVectors = false;

    public bool EnableLog = false;


    public int recordsNumber = 0;

    private void Update()
    {
        recordsNumber = recording.RecordNumber();
        transform.position = FixConverter.ToFixVec3(Position);
    }

    protected void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
    {
        Position = position;
        Velocity = velocity;
        recording.Add(Velocity, FixWorld.time, Position);
    }

    protected void ChangePositionAndVelocity(FixVec3 position, FixVec3 velocity)
    {
        recording.Add(Velocity, FixWorld.time, Position);
        DrawLine(Position, position, Color.blue);
        Position = position;
        Velocity = velocity;
    }

    protected void PositionCorrection(FixVec3 newPosition)
    {
        DrawLine(Position, newPosition, Color.red);
        recording.Add(Velocity, FixWorld.time, Position);
        Position = newPosition;
    }

    protected void VelocityCorrection(FixVec3 newVelocity)
    {
        recording.Add(Velocity, FixWorld.time, Position);
        Velocity = newVelocity;
    }

    protected void Step()
    {
        Position += Velocity * FixWorld.deltaTime;
    }

    protected void StepBack()
    {
        Position -= Velocity * FixWorld.deltaTime;
    }

    protected void Accelerate(FixVec3 sumForce)
    {
        Velocity += sumForce * FixWorld.deltaTime;
    }

    protected void AccelerateBack(FixVec3 sumForce)
    {
        Velocity -= sumForce * FixWorld.deltaTime;
    }

    protected void SetNow()
    {
        SetRecord(recording.Get(FixWorld.time));
    }

    protected void SetRecord(FixRecording.Record record)
    {
        if (record != null)
        {
            Velocity = record.velocity;
            Position = record.position;
            Kinematic = record.kinematic;
        }
    }

    protected void DrawLine(FixVec3 pointA, FixVec3 pointB, Color color)
    {
        if (!DrawVectors) return;
        Debug.DrawLine(FixConverter.ToFixVec3(pointB), FixConverter.ToFixVec3(pointA), color, Time.fixedDeltaTime, false);
    }

    protected void DrawVector(FixVec3 normal, Color color)
    {
        if (!DrawVectors) return;
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, Time.fixedDeltaTime, false);
    }

    protected void Log()
    {
        if (EnableLog)
            Debug.Log(Position + " :  " + Velocity + " kinematic: " + Kinematic);
    }
}
