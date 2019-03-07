using UnityEngine;
using System.Collections;
using FixedPointy;

public class RotatableBoxFixCollider : FixCollider
{
    public Vector3 scaleAdjustment = new Vector3(1, 1, 1);
    private FixVec3 scale = FixVec3.Zero;
    private FixTrans3 rotateMatrix = FixTrans3.Identity;
    private FixTrans3 inverseRotateMatrix = FixTrans3.Identity;

    private void Start()
    {
        float x = Mathf.Abs(transform.localScale.x * scaleAdjustment.x);
        float y = Mathf.Abs(transform.localScale.y * scaleAdjustment.y);
        float z = Mathf.Abs(transform.localScale.z * scaleAdjustment.z);
        scale = FixConverter.ToFixVec3(new Vector3(x, y, z));

        x = Mathf.Abs((float)scale.X / scaleAdjustment.x);
        y = Mathf.Abs((float)scale.Y / scaleAdjustment.y);
        z = Mathf.Abs((float)scale.Z / scaleAdjustment.z);
        transform.localScale = new Vector3(x,y,z);
        DrawLine(GetPosition(), RightUp, Color.blue);
        DrawLine(GetPosition(), RightDown, Color.blue);
        DrawLine(GetPosition(), LeftUp, Color.blue);
        DrawLine(GetPosition(), LeftDown, Color.blue);
        FixVec3 rotation = FixConverter.ToFixVec3BigNumber(transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Euler(FixConverter.ToFixVec3(rotation));
        rotateMatrix = FixTrans3.MakeRotation(rotation);
        inverseRotateMatrix = FixTrans3.MakeRotation(rotation * -1);
    }

    private FixVec3 RightUp
    {
        get
        {
            return GetPosition() + rotateMatrix * (FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 RightDown
    {
        get
        {
            return GetPosition() + rotateMatrix * (FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 LeftUp
    {
        get
        {
            return GetPosition() + rotateMatrix * (-FixVec3.UnitX * scale.X / 2 + FixVec3.UnitY * scale.Y / 2);
        }
    }

    private FixVec3 LeftDown
    {
        get
        {
            return GetPosition() + rotateMatrix * (-FixVec3.UnitX * scale.X / 2 - FixVec3.UnitY * scale.Y / 2);
        }
    }

    protected override Collision Collide(FixCollider other)
    {
        FixVec3 innerPosition = ConvertToInner(other.GetPosition());
        FixVec3 innerDirection = innerPosition;
        innerDirection = ScaleVector(innerDirection);
        if (FixMath.Abs(innerDirection.Y) >= FixMath.Abs(innerDirection.X) && innerDirection.Y > 0)
        {
            // +Y
            if (other.CollideSegment(LeftUp, RightUp))
            {
                FixVec3 normal = rotateMatrix * FixVec3.UnitY;
                FixVec3 realPoint = ConvertToRealWorld(new FixVec3(innerPosition.X, scale.Y / 2, innerPosition.Z));
                FixVec3 overlap = other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitY);
                DrawLineShort(other.GetPosition(), GetPosition(), Color.cyan);
                return new Collision(normal, overlap);
            }
        }
        else if (FixMath.Abs(innerDirection.Y) >= FixMath.Abs(innerDirection.X) && innerDirection.Y < 0)
        {
            // -Y
            if (other.CollideSegment(LeftDown, RightDown))
            {
                FixVec3 normal = rotateMatrix * FixVec3.UnitY * -1;
                FixVec3 realPoint = ConvertToRealWorld(new FixVec3(innerPosition.X, -scale.Y / 2, innerPosition.Z));
                FixVec3 overlap = other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitY * -1);
                DrawLineShort(other.GetPosition(), GetPosition(), Color.cyan);
                return new Collision(normal, overlap);
            }
        }
        else if (FixMath.Abs(innerDirection.Y) <= FixMath.Abs(innerDirection.X) && innerDirection.X > 0)
        {
            // +X
            if (other.CollideSegment(RightDown, RightUp))
            {
                FixVec3 realPoint = ConvertToRealWorld(new FixVec3(scale.X / 2, innerPosition.Y, innerPosition.Z));
                FixVec3 overlap = other.GetIntersectionFromPoint(realPoint, rotateMatrix * FixVec3.UnitX);
                FixVec3 normal = rotateMatrix * FixVec3.UnitX;
                DrawLineShort(other.GetPosition(), GetPosition(), Color.cyan);
                return new Collision(normal, overlap);
            }
        }
        else
        {
            // -X
            if (other.CollideSegment(LeftDown, LeftUp))
            {
                FixVec3 realPoint2 = ConvertToRealWorld(new FixVec3(-scale.X / 2, innerPosition.Y, innerPosition.Z));
                FixVec3 overlap = other.GetIntersectionFromPoint(realPoint2, rotateMatrix * FixVec3.UnitX * -1);
                FixVec3 normal = rotateMatrix * FixVec3.UnitX * -1;
                DrawLineShort(other.GetPosition(), GetPosition(), Color.cyan);
                return new Collision(normal, overlap);
            }
        }
        return null;
    }

    public override bool CollidePoint(FixVec3 point)
    {
        FixVec3 realPoint = ConvertToInner(point);
        Fix XPos = FixMath.Abs(realPoint.X);
        Fix YPos = FixMath.Abs(realPoint.Y);

        if ((XPos <= (scale.X / 2)) && (YPos <= (scale.Y / 2)))
        {
            DrawLineShort(point, GetPosition(), Color.green);
            return true;
        }
        return false;
    }

    public override bool CollideSegment(FixVec3 pointA, FixVec3 pointB)
    {
        Fix lengthInput = FixMath.Abs(pointA.X - pointB.X) + FixMath.Abs(pointA.Y - pointB.Y);
        Fix maxLength = FixMath.Max( scale.X, scale.Y);
       
        if (FixMath.Abs((pointA - GetPosition()).GetMagnitude()) > (lengthInput + maxLength) &&
            FixMath.Abs((pointB - GetPosition()).GetMagnitude()) > (lengthInput + maxLength))
        {
            return false;
        }
        // Optimalization, if its in there Do Not Check Intersect
        if (CollidePoint(pointA))
        {
            return true;
        }
        if (CollidePoint(pointB))
        {
            return true;
        }

        if (HelpFixMath.DoIntersect(pointA, pointB, LeftDown, LeftUp))
        {
            DrawLineShort(pointA, LeftDown, Color.red);
            DrawLineShort(pointB, LeftUp, Color.red);
            return true;
        }

        if (HelpFixMath.DoIntersect(pointA, pointB, LeftDown, RightDown))
        {
            DrawLineShort(pointA, LeftDown, Color.red);
            DrawLineShort(pointB, RightDown, Color.red);
            return true;
        }

        if (HelpFixMath.DoIntersect(pointA, pointB, RightDown, RightUp))
        {
            DrawLineShort(pointA, RightDown, Color.red);
            DrawLineShort(pointB, RightUp, Color.red);
            return true;
        }

        if (HelpFixMath.DoIntersect(pointA, pointB, RightUp, LeftUp))
        {
            DrawLineShort(pointA, RightUp, Color.red);
            DrawLineShort(pointB, LeftUp, Color.red);
            return true;
        }

        return false;
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
        return inverseRotateMatrix * (point - GetPosition());
    }

    private FixVec3 ConvertToInnerVector(FixVec3 dir)
    {
        return inverseRotateMatrix * dir;
    }

    private FixVec3 ScaleVector(FixVec3 dir)
    {
        return new FixVec3(dir.X * scale.Y, dir.Y * scale.X, dir.Z);
    }

    private FixVec3 ConvertToRealWorld(FixVec3 point)
    {
        return (rotateMatrix * point) + GetPosition();
    }

    private FixVec3 ConvertToRealWorldVector(FixVec3 dir)
    {
        return rotateMatrix * dir;
    }   
}
