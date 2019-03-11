using UnityEngine;
using System.Collections;
using FixedPointy;

public class PlayerRecord : Record
{
    public bool collided;
    public PlayerRecord(FixVec3 velocity, Fix time, FixVec3 position, bool collided): base(velocity, time, position, false)
    {
        this.collided = collided;
    }
}
