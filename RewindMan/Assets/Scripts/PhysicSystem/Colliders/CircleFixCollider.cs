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

    public override Collision GetCollision(FixCollider other)
    {
        FixVec3 normal = (other.GetPosition() - GetPosition()).Normalize();
        FixVec3 collisionPoint = GetPosition() + (normal) * radius;
        if (other.CollidePoint(collisionPoint))
        {
            FixVec3 myPoint = GetPoint(other.GetPosition() - GetPosition());
            FixVec3 overlap = other.GetIntersectionFromPoint(myPoint, other.GetPosition() - GetPosition());
            return new Collision(normal, overlap);
        }
        return null;
    }

    public override bool CollidePoint(FixVec3 point)
    {
        return (point - GetPosition()).GetMagnitude() < radius;
    }

    public override bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        if (CollidePoint(pointA)) return true;
        if (CollidePoint(pointB)) return true;
        Fix dist = HelpFixMath.PointLineDistance(GetPosition(), pointA, pointB);
        if (FixMath.Abs(dist) < radius)
        {
            FixVec3 dir = pointA - pointB;
            dir = new FixVec3(dir.Y, dir.X * -1, dir.Z);
            FixVec3 collidePoint = GetPosition() - dir.Normalize() * radius;
            FixVec3 collidePoint2 = GetPosition() + dir.Normalize() * radius;
            if (HelpFixMath.DoIntersect(pointA, pointB, collidePoint, collidePoint2))
            {
                DrawLineShort(pointA,GetPosition(), Color.yellow);
                DrawLineShort(pointB, GetPosition(), Color.yellow);
                return true;
            }
        }
        return false;
    }

    public override FixVec3 GetIntersectionFromPoint(FixVec3 otherPoint, FixVec3 dir)
    {
        FixVec3 myPoint = GetPoint(otherPoint - GetPosition());
        return (otherPoint - myPoint);
    }

    private FixVec3 GetPoint(FixVec3 direction)
    {
        return (GetPosition() + direction.Normalize() * radius);
    }
}
