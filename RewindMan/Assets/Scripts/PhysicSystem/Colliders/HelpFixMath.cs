using UnityEngine;
using System.Collections;
using FixedPointy;

public class HelpFixMath
{
    public static Fix PointLineDistance(FixVec3 Point, FixVec3 LineA, FixVec3 LineB)
    {
        FixVec3 UL = (LineB - LineA).Normalize();
        FixVec3 W = Point - LineA;
        return UL.Dot(W);
    }

    public static bool OnSegment(FixVec3 p, FixVec3 q, FixVec3 r)
    {
        if (q.X <= FixMath.Max(p.X, r.X) && q.X >= FixMath.Min(p.X, r.X) &&
            q.Y <= FixMath.Max(p.Y, r.Y) && q.Y >= FixMath.Min(p.Y, r.Y))
            return true;
        return false;
    }

    public static int Orientation(FixVec3 p, FixVec3 q, FixVec3 r)
    {
        Fix val = (q.Y - p.Y) * (r.X - q.X) -
                  (q.X - p.X) * (r.Y - q.Y);
        if (val == 0) return 0;
        return (val > 0) ? 1 : 2;
    }

    public static bool DoIntersect(FixVec3 p1, FixVec3 q1, FixVec3 p2, FixVec3 q2)
    {
        int o1 = Orientation(p1, q1, p2);
        int o2 = Orientation(p1, q1, q2);
        int o3 = Orientation(p2, q2, p1);
        int o4 = Orientation(p2, q2, q1);

        if (o1 != o2 && o3 != o4)
            return true;

    
        if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

        // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
        if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

        // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
        if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

        // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
        if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

        return false; // Doesn't fall in anY of the above cases 
    }

    public static FixVec3 Project(FixVec3 from, FixVec3 to)
    {
        if (to.GetMagnitude() == 0) return FixVec3.Zero;
       return to * (from.Dot(to) / to.Dot(to));
    }

}

