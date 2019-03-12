using UnityEngine;
using System.Collections;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class MovingObject : RecordedObjectOther
{
    public Vector3 startDistance = new Vector3(0, 1, 0);
    public float startSpeed = 2.0f;
    private Fix speed;
    private FixVec3 start;
    private FixVec3 distance;
    private FixCollider fixCollider;

    protected override void Start()
    {
        base.Start();
        speed = FixConverter.ToFix(startSpeed);
        start = state.position;
        distance = FixConverter.ToFixVec3(startDistance);
        state = MovingRecord.RecordFromBase(state, 0);
        fixCollider = GetComponent<FixCollider>();
    }

    public virtual void Move()
    {
        FixVec3 position = state.position;
        Increment(FixWorldComplex.deltaTime * speed);
        FixVec3 position2 = GetPosition();
        state.velocity = (position2 - position) * (1 / FixWorldComplex.deltaTime);
        state.position = position2;
        fixCollider.SetPositionAndVelocity(state.position, state.velocity);
    }

    private FixVec3 GetPosition()
    {
        return start + distance * (FixMath.Sin(GetAngle() * 100) + 1);
    }

    private void Increment(Fix inc)
    {
        ((MovingRecord)state).angle += inc;
    }

    private Fix GetAngle()
    {
        return ((MovingRecord)state).angle;
    }
}
