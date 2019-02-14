using UnityEngine;
using System.Collections;
using FixedPointy;

public class FixCollider : MonoBehaviour
{
    protected FixVec3 position;

    public void SetPosition(FixVec3 position)
    {
        this.position = position;
    }

    public FixVec3 GetPosition()
    {
        return position;
    }

    public virtual bool Collide(FixCollider other)
    {
       return false;
    }

    public virtual bool CollidePoint(FixVec3 point)
    {
        return false;
    }

    public virtual bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        return false;
    }

    public virtual FixVec3 GetNormal(FixCollider other)
    {
        return (other.position - position).Normalize();
    }

    public virtual FixVec3 GetIntersection(FixCollider other)
    {
        return (other.position - position);
    }

    public virtual FixVec3 GetIntersectionFromPoint(FixVec3 point)
    {
        return (point - position);
    }
}
