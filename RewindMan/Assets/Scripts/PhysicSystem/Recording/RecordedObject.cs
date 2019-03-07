using UnityEngine;
using FixedPointy;

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

    protected void ResetRecording()
    {
        recording.Reset();
    }

    protected void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
    {
        Position = position;
        Velocity = velocity;
        recording.Add(Velocity, FixWorld.time, Position, DrawVectors);
    }

    protected void PositionCorrection(FixVec3 newPosition)
    {
        DrawLine(Position, newPosition, Color.red);
        recording.Add(Velocity, FixWorld.time, Position, DrawVectors);
        Position = newPosition;
    }

    protected void VelocityCorrection(FixVec3 newVelocity)
    {
        recording.Add(Velocity, FixWorld.time, Position, DrawVectors);
        Velocity = newVelocity;
        if (newVelocity.GetMagnitude() < FixWorld.deltaTime * FixWorld.gravity.GetMagnitude()) newVelocity = FixVec3.Zero;
    }

    protected void Step(bool back = false)
    {
        Position += Velocity * FixWorld.deltaTime * (back ? -1 : 1) ;
    }

    protected void Accelerate(FixVec3 sumForce, bool back = false)
    {
        Velocity += sumForce * FixWorld.deltaTime * (back ? -1 : 1);
    }

    protected void SetNow()
    {
        FixRecording.Record record = recording.Get(FixWorld.time);
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
}
