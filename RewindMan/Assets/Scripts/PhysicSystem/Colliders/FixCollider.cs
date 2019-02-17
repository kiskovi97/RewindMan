using UnityEngine;
using System.Collections;
using FixedPointy;

public abstract class FixCollider : MonoBehaviour
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

    public abstract Collision GetCollision(FixCollider other);

    public abstract bool CollidePoint(FixVec3 point);

    public abstract bool CollideSegment(FixVec3 pointA, FixVec3 pointB);

    public abstract FixVec3 GetIntersectionFromPoint(FixVec3 point, FixVec3 dir);

    // DebugLines
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
