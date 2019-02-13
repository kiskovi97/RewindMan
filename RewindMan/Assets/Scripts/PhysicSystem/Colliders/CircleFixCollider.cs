using UnityEngine;
using System.Collections;
using FixedPointy;

public class CircleFixCollider : FixCollider
{
    public float startingRadius = 0.5f;
    private Fix radius = Fix.Zero;

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
        return dist < radius;
    }

    public override FixVec3 GetNormal(FixCollider other)
    {
        return (other.GetPosition() - position).Normalize();
    }
}
