using UnityEngine;
using FixedPointy;

public class FixConverter
{
    public static Fix divide = 2048;
    public static Fix ToFix(float from)
    {
        Fix tmp = (int) (from * (float) divide);
        Fix output = tmp / divide;
        return (FixConst)from;

    } 
    public static FixVec3 ToFixVec3(Vector3 from)
    {
        Fix X = ToFix(from.x);
        Fix Y = ToFix(from.y);
        Fix Z = ToFix(from.z);
        return new FixVec3(X,Y,Z);
    }


    public static FixVec3 ToFixVec3BigNumber(Vector3 from)
    {
        Fix X = (int)(from.x);
        Fix Y = (int)(from.y);
        Fix Z = (int)(from.z);
        return new FixVec3(X, Y, Z);
    }

    public static Vector3 ToFixVec3(FixVec3 from)
    {
        float X = (float)(from.X);
        float Y = (float)(from.Y);
        float Z = (float)(from.Z);
        return new Vector3(X, Y, Z);
    }

    public static Fix RadToAngle(Fix rad)
    {
        Fix Angle = 180;
        return (rad / FixMath.PI) * Angle;
    }
}
