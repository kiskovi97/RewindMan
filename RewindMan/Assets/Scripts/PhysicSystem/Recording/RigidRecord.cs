using UnityEngine;
using System.Collections;
using FixedPointy;

public class RigidRecord : Record
{
    public int collided;
    public FixVec3 prevVelocity = FixVec3.Zero;
    public RigidRecord(FixVec3 velocity, Fix time, FixVec3 position, int collided, FixVec3 prevVelocity) : base(velocity, time, position, false)
    {
        this.collided = collided;
        this.prevVelocity = prevVelocity;
    }

    public override Record Copy()
    {
        return new RigidRecord(this);
    }

    private RigidRecord(RigidRecord record) : base(record)
    {
        this.collided = record.collided;
        this.prevVelocity = record.prevVelocity;
    }

    public static Record RecordFromBase(Record state, int collided, FixVec3 prevVelocity)
    {
        return new RigidRecord(state.velocity, state.time, state.position, collided, prevVelocity);
    }
}
