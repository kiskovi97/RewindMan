using UnityEngine;
using System.Collections;
using FixedPointy;

public class RigidRecord : Record
{
    public int collided;
    public RigidRecord(FixVec3 velocity, Fix time, FixVec3 position, int collided) : base(velocity, time, position, false)
    {
        this.collided = collided;
    }

    public override Record Copy()
    {
        return new RigidRecord(this);
    }

    private RigidRecord(RigidRecord record) : base(record)
    {
        this.collided = record.collided;
    }

    public static Record RecordFromBase(Record state, int collided)
    {
        return new RigidRecord(state.velocity, state.time, state.position, collided);
    }
}
