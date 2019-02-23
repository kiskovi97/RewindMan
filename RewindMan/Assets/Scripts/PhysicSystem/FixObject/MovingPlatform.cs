using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;
[RequireComponent(typeof(FixCollider))]
class MovingPlatform : RecordedObject, FixObject
{
    private FixCollider fixCollider;
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public float period = 1.0f;
    public Fix periodTime;
    public Fix periodStatus = Fix.Zero;

    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();
        FixVec3 position = (FixConverter.ToFixVec3(transform.position));
        FixVec3 velocity = FixConverter.ToFixVec3(startVelocity);
        periodTime = FixConverter.ToFix(period);
        SetPositionAndVelocity(position, velocity);
        fixCollider.SetPosition(Position);
    }

    public void Collide(Collision[] collisions)
    {
        return;
    }

    public void CollideBack(Collision[] collisions)
    {
        return;
    }

    public FixCollider Collider()
    {
        return fixCollider;
    }

    public Collision GetCollision(FixObject other)
    {
        FixCollider collider = other.Collider();

        if (collider == null) return null;

        Collision collision = fixCollider.GetCollision(collider);

        if (collision != null) collision.SetObjectsValues(Velocity, IsStatic(), Position);

        return collision;
    }

    public bool IsStatic()
    {
        return true;
    }

    public void Move()
    {
        periodStatus += FixWorld.deltaTime;
        if (periodStatus >= periodTime)
        {
            periodStatus = Fix.Zero;
            VelocityCorrection(Velocity * -1);
        }
        Step();
        fixCollider.SetPosition(Position);
    }

    public void MoveBackwards()
    {
        StepBack();
        fixCollider.SetPosition(Position);
        if (periodStatus <= Fix.Zero)
        {
            periodStatus = periodTime;
        }
        SetNow();
        periodStatus -= FixWorld.deltaTime;
    }
}
