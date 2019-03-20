using UnityEngine;
using System.Collections;
using FixedPointy;

public class Collision
{
    public string tag;
    public FixVec3 Normal;
    public FixVec3 Overlap;

    public FixVec3 savedVelocity;
    public bool isStatic;
    public FixVec3 position;

    public Collision(FixVec3 Normal, FixVec3 overlap)
    {
        this.Normal = Normal;
        this.Overlap = overlap;
    }

    public void SetObjectsValues(FixVec3 savedVelocity, bool isStatic, FixVec3 position, string tag)
    {
        this.savedVelocity = savedVelocity;
        this.isStatic = isStatic;
        this.position = position;
        this.tag = tag;
    }
}
