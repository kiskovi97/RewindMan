using UnityEngine;
using System.Collections;
using FixedPointy;

public class RotatableBoxFixCollider : FixCollider
{

    public Vector3 scaleAdjustment = new Vector3(1, 1, 1);
    private FixVec3 scale = FixVec3.Zero;
    private FixTrans3 rotateMatrix = FixTrans3.Identity;
    private FixTrans3 inverseRotateMatrix = FixTrans3.Identity;
    public bool DrawLines = false;

    private void Start()
    {
        float x = Mathf.Abs(transform.localScale.x * scaleAdjustment.x);
        float y = Mathf.Abs(transform.localScale.y * scaleAdjustment.y);
        float z = Mathf.Abs(transform.localScale.z * scaleAdjustment.z);
        scale = FixConverter.ToFixVec3(new Vector3(x, y, z));
        transform.localScale = FixConverter.ToFixVec3(scale);
        FixVec3 rotation = FixConverter.ToFixVec3BigNumber(transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Euler(FixConverter.ToFixVec3(rotation));

        rotateMatrix = FixTrans3.MakeRotation(rotation);
        inverseRotateMatrix = FixTrans3.MakeRotation(rotation * -1);
    }

    private FixVec3 RightUp
    {
        get
        {
            return position + rotateMatrix * (FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 RightDown
    {
        get
        {
            return position + rotateMatrix * (FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 LeftUp
    {
        get
        {
            return position + rotateMatrix * (- FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 LeftDown
    {
        get
        {
            return position + rotateMatrix * (- FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2);
        }
    }

    public override bool Collide(FixCollider other)
    {
        FixVec3 direction = inverseRotateMatrix * (other.GetPosition() - position);

        direction = new FixVec3(direction.X * scale.Y, direction.Y * scale.X, direction.Z);
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
        FixVec3 realPoint = inverseRotateMatrix * (point - position);
        Fix XPos = FixMath.Abs(realPoint.X);
        Fix YPos = FixMath.Abs(realPoint.Y);

        return (XPos <= scale.X / 2) && (YPos <= scale.Y / 2);
    }

    public override bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        if ((pointA - position).GetMagnitude() > (scale.X + scale.Y) &&
            (pointB - position).GetMagnitude() > (scale.X + scale.Y)) return false;
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

        FixVec3 direction = inverseRotateMatrix * (other.GetPosition() - position);
        direction = new FixVec3(direction.X * scale.Y, direction.Y * scale.X, direction.Z);
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            return rotateMatrix * FixVec3.UnitY;
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            return rotateMatrix * FixVec3.UnitY * -1;
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            return rotateMatrix * FixVec3.UnitX;
        }
        return rotateMatrix * FixVec3.UnitX * -1;
    }

    public override FixVec3 GetIntersection(FixCollider other)
    {
        FixVec3 pos = ConvertToInner(other.GetPosition());
        FixVec3 direction = pos;
        direction = new FixVec3(direction.X * scale.Y, direction.Y * scale.X, direction.Z);
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(pos.X, scale.Y / 2, pos.Z));
            return other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitY);
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(pos.X, -scale.Y / 2, pos.Z));
            return other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitY * -1);
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(scale.X / 2, pos.Y, pos.Z));
            return other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitX);
        }
        FixVec3 realPoint2 = ConvertToRealWorld(new FixVec3(-scale.X / 2, pos.Y, pos.Z));
        return other.GetIntersectionFromPoint(realPoint2, rotateMatrix * FixVec3.UnitX * -1);
    }

    public override FixVec3 GetIntersectionFromPoint(FixVec3 otherPoint, FixVec3 dir)
    {
        FixVec3 pos = ConvertToInner(otherPoint);
        FixVec3 direction = new FixVec3(dir.X * scale.Y, dir.Y * scale.X, dir.Z) * -1;
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y > 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(pos.X, scale.Y / 2, pos.Z));
            return otherPoint - realPoint;
        }
        if (FixMath.Abs(direction.Y) >= FixMath.Abs(direction.X) && direction.Y < 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(pos.X, -scale.Y / 2, pos.Z));
            return otherPoint - realPoint;
        }
        if (FixMath.Abs(direction.Y) <= FixMath.Abs(direction.X) && direction.X > 0)
        {
            FixVec3 realPoint = ConvertToRealWorld(new FixVec3(scale.X / 2, pos.Y, pos.Z));
            return otherPoint - realPoint;
        }
        FixVec3 realPoint2 = ConvertToRealWorld(new FixVec3(-scale.X / 2, pos.Y, pos.Z));
        return otherPoint - realPoint2;
    }

    private FixVec3 ConvertToInner(FixVec3 point)
    {
        return inverseRotateMatrix * (point - position);
    }

    private FixVec3 ConvertToRealWorld(FixVec3 point)
    {

        return (rotateMatrix * point) + position;
    }
}
