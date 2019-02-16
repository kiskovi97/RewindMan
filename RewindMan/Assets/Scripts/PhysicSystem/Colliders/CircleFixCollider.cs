using UnityEngine;
using System.Collections;
using FixedPointy;

public class CircleFixCollider : FixCollider
{
    public float startingRadius = 0.5f;
    private Fix radius = Fix.Zero;
    public bool DrawLines = false;

    private void Start()
    {
        radius = FixConverter.ToFix(startingRadius);
    }

    public override bool Collide(FixCollider other)
    {
        FixVec3 collidePoint = position + (other.GetNormal(this) * -1) * radius;
        return other.CollidePoint(collidePoint);
    }

    public override bool CollidePoint(FixVec3 point)
    {
        return (point - position).GetMagnitude() < radius;
    }

    public override bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        if (CollidePoint(pointA)) return true;
        if (CollidePoint(pointB)) return true;
        Fix dist = HelpFixMath.PointLineDistance(position, pointA, pointB);
        if (FixMath.Abs(dist) < radius)
        {
            if (DrawLines)
            {
                Debug.DrawLine(FixConverter.ToFixVec3(pointA), FixConverter.ToFixVec3(position), Color.yellow);
                Debug.DrawLine(FixConverter.ToFixVec3(pointB), FixConverter.ToFixVec3(position), Color.yellow);

            }
        }
        return FixMath.Abs(dist) < radius;
    }

    public override FixVec3 GetNormal(FixCollider other)
    {
        return (other.GetPosition() - position).Normalize();
    }

    public override FixVec3 GetIntersection(FixCollider other)
    {
        FixVec3 myPoint = GetPoint(other.GetPosition() - position);
        return other.GetIntersectionFromPoint(myPoint, other.GetPosition() - position);
    }
    
    public override FixVec3 GetIntersectionFromPoint(FixVec3 otherPoint, FixVec3 dir)
    {
        FixVec3 myPoint = GetPoint(otherPoint - position);
        return (otherPoint - myPoint);
    }

    FixVec3 GetPoint(FixVec3 direction)
    {
        return (position + direction.Normalize() * radius);
    }
}
