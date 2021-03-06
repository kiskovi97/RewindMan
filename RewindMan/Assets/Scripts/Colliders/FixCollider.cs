﻿using UnityEngine;
using System.Collections;
using FixedPointy;
using System.Threading;

public abstract class FixCollider : MonoBehaviour
{
    public bool Draw = false;
    public bool Enabled = true;
    public bool isStatic = true;
    public float massIn = 1f;
    public Fix mass = 1;
    private FixVec3 position;
    private FixVec3 velocity;
    private string myTag;
    static Thread mainThread = Thread.CurrentThread;

    protected virtual void Start()
    {
        velocity = FixVec3.Zero;
        position = FixConverter.ToFixVec3(transform.position);
        myTag = tag;
        mass = FixConverter.ToFix(massIn);
    }

    public void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }

    public FixVec3 GetPosition()
    {
        return position;
    }

    public Collision GetCollision(FixCollider other)
    {
        if (!Enabled) return null;
        Collision collision = Collide(other);
        if (collision != null)
            collision.SetObjectsValues(velocity, isStatic, position, myTag, mass);
        return collision;
    }

    protected abstract Collision Collide(FixCollider other);

    public abstract bool CollidePoint(FixVec3 point);

    public abstract bool CollideSegment(FixVec3 pointA, FixVec3 pointB);

    public abstract FixVec3 GetIntersectionFromPoint(FixVec3 point, FixVec3 dir);

    // DebugLines
    protected void DrawLine(FixVec3 pointA, FixVec3 pointB, Color color)
    {
        if (Draw && Thread.CurrentThread == mainThread)
            Debug.DrawLine(FixConverter.ToFixVec3(pointA), FixConverter.ToFixVec3(pointB), color, 10000f, false);
    }

    protected void DrawLineShort(FixVec3 pointA, FixVec3 pointB, Color color)
    {
        if (Draw && Thread.CurrentThread == mainThread)
            Debug.DrawLine(FixConverter.ToFixVec3(pointA), FixConverter.ToFixVec3(pointB), color, 0.0001f, false);
    }
}
