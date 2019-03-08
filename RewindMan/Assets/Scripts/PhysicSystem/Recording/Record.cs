using UnityEngine;
using System.Collections;
using FixedPointy;

public class Record
{
    public FixVec3 velocity;
    public Fix time;
    public FixVec3 position;
    public bool kinematic = false;
    public Record(FixVec3 velocity, Fix time, FixVec3 position, bool kinematic = false)
    {
        this.velocity = velocity;
        this.time = time;
        this.position = position;
        this.kinematic = kinematic;
    }
    public bool Equals(Record other)
    {
        return velocity.Equals(other.velocity) && position.Equals(other.position) && velocity.GetMagnitude() < new Fix(8);
    }
    public override string ToString()
    {
        return velocity.ToString();
    }
}
