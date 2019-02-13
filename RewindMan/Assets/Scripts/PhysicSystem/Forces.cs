using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class Forces 
{
    private List<FixVec3> impulses = new List<FixVec3>();

    public void Clear()
    {
        impulses.Clear();
    }

    public void AddImpulse(FixVec3 force)
    {
        impulses.Add(force);
    }

    public FixVec3 GetSumForces()
    {
        FixVec3 force = FixVec3.Zero;
        foreach (FixVec3 impulse in impulses) force += impulse;
        return force;
    }
    
}
