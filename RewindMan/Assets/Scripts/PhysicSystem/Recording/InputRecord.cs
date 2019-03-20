using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FixedPointy;

public class InputRecord
{
    public bool left;
    public bool right;
    public bool up;
    public Fix time;
    
    public InputRecord(bool left, bool right, bool up, Fix time)
    {
        this.right = right;
        this.left = left;
        this.up = up;
        this.time = time;
    }

    public InputRecord()
    {
        this.right = false;
        this.left = false;
        this.up = false;
        this.time = Fix.Zero;
    }

    public override string ToString()
    {
        return right + " time: " + time;
    }

    public InputRecord Copy()
    {
        return new InputRecord(left, right, up, time);
    }
}
