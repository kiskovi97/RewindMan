using UnityEngine;
using System.Collections;
using FixedPointy;

public class PlayerRecord : Record
{
    public int collided;
    public PlayerRecord(FixVec3 velocity, Fix time, FixVec3 position, int collided) : base(velocity, time, position, false)
    {
        this.collided = collided;
    }

    public override Record Copy()
    {
        return new PlayerRecord(this);
    }

    private PlayerRecord(PlayerRecord record) : base(record)
    {
        this.collided = record.collided;
    }

    public static Record RecordFromBase(Record state, int collided)
    {
        return new PlayerRecord(state.velocity, state.time, state.position, collided);
    }
}
