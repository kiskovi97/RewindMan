using UnityEngine;
using System.Collections;
using FixedPointy;

public class FixCollider : MonoBehaviour
{
    public bool Draw = false;
    private FixVec3 position;

    public void SetPosition(FixVec3 position)
    {
        this.position = position;
    }

    public FixVec3 GetPosition()
    {
        return position;
    }

    public virtual Collision GetCollision(FixCollider other)
    {
        return null;
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

    public virtual FixVec3 GetIntersectionFromPoint(FixVec3 point, FixVec3 dir)
    {
        return (point - position);
    }

    protected void DrawLine(FixVec3 pointA, FixVec3 pointB, Color color)
    {
        if (Draw)
            Debug.DrawLine(FixConverter.ToFixVec3(pointA), FixConverter.ToFixVec3(pointB), color, 10000f, false);
    }

    protected void DrawLineShort(FixVec3 pointA, FixVec3 pointB, Color color)
    {
        if (Draw)
            Debug.DrawLine(FixConverter.ToFixVec3(pointA), FixConverter.ToFixVec3(pointB), color, Time.fixedDeltaTime, false);
    }
}
