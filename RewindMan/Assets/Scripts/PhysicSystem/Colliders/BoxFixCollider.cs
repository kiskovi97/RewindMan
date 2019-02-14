using UnityEngine;
using System.Collections;
using FixedPointy;

public class BoxFixCollider : FixCollider
{
    public Vector3 scaleAdjustment = new Vector3(1, 1, 1);
    private FixVec3 scale = FixVec3.Zero;

    private void Start()
    {
        float x = Mathf.Abs(transform.localScale.x * scaleAdjustment.x);
        float y = Mathf.Abs(transform.localScale.y * scaleAdjustment.y);
        float z = Mathf.Abs(transform.localScale.z * scaleAdjustment.z);
        scale = FixConverter.ToFixVec3(new Vector3(x,y,z));
    }

    private FixVec3 RightUp
    {
        get
        {
            return position + FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2;
        }
    }

    private FixVec3 RightDown
    {
        get
        {
            return position + FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2;
        }
    }

    private FixVec3 LeftUp
    {
        get
        {
            return position - FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2;
        }
    }

    private FixVec3 LeftDown
    {
        get
        {
            return position - FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2;
        }
    }

    public override bool Collide(FixCollider other)
    {
        FixVec3 direction = other.GetPosition() - position;
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            return other.CollideSegment(LeftUp, RightUp);
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            return other.CollideSegment(LeftDown, RightDown);
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            return other.CollideSegment(RightDown, RightUp);
        }
        return other.CollideSegment(LeftDown, LeftUp);
    }

    public override bool CollidePoint(FixVec3 point)
    {
        Fix XPos = FixMath.Abs(point.X - position.X);
        Fix YPos = FixMath.Abs(point.Y - position.Y);

        return (XPos < scale.X / 2) && (YPos < scale.Y / 2);
    }

    public override bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        // Optimalization, if its in there Do Not Check Intersect
        if (CollidePoint(pointA)) return true;
        if (CollidePoint(pointB)) return true;

        return HelpFixMath.DoIntersect(pointA, pointB, LeftDown, LeftUp) ||
            HelpFixMath.DoIntersect(pointA, pointB, LeftDown, RightDown) ||
            HelpFixMath.DoIntersect(pointA, pointB, RightDown, RightUp) ||
            HelpFixMath.DoIntersect(pointA, pointB, RightUp, LeftUp);
    }

    public override FixVec3 GetNormal(FixCollider other)
    {
        FixVec3 direction = other.GetPosition() - position;
        direction = new FixVec3(direction.X * scale.Y, direction.Y * scale.X, direction.Z);
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            return FixVec3.UnitY;
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            return FixVec3.UnitY * -1;
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            return FixVec3.UnitX;
        }
        return FixVec3.UnitX * -1;
    }

    public override FixVec3 GetIntersection(FixCollider other)
    {
        FixVec3 pos = other.GetPosition();
        FixVec3 direction = other.GetPosition() - position;
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            return GetIntersectionFromPoint(new FixVec3(pos.X, LeftUp.Y, pos.Z));
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            return GetIntersectionFromPoint(new FixVec3(pos.X, LeftDown.Y, pos.Z));
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            return GetIntersectionFromPoint(new FixVec3(RightDown.X, pos.Y, pos.Z));
        }
        return GetIntersectionFromPoint(new FixVec3(LeftDown.X, pos.Y, pos.Z));
    }

    public override FixVec3 GetIntersectionFromPoint(FixVec3 otherPoint)
    {
        /**
         * TODO */
        return FixVec3.Zero;
    }
}
