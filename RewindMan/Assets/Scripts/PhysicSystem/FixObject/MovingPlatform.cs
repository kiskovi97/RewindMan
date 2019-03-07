using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;
[RequireComponent(typeof(FixCollider))]
class MovingPlatform : MonoBehaviour, FixObject
{
    public Vector3 startDistance = new Vector3(0, 1, 0);
    public float startSpeed = 1.0f;

    private FixCollider fixCollider;
    private Fix speed = Fix.Zero;
    private FixVec3 distance = FixVec3.Zero;
    private FixVec3 start = FixVec3.Zero;
    private FixVec3 velocity = FixVec3.Zero;
    private Fix angle = Fix.Zero;


    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();
        start = (FixConverter.ToFixVec3(transform.position));
        distance = FixConverter.ToFixVec3(startDistance);
        speed = FixConverter.ToFix(startSpeed);
        transform.position = FixConverter.ToFixVec3(GetPosition());
        fixCollider.SetPosition(GetPosition());
    }

    private FixVec3 GetPosition()
    {
        return start + distance * (FixMath.Sin(angle * 100) + 1);
    }

    public void Collide(Collision[] collisions)
    {
        return;
    }

    public void CollideBack(Collision[] collisions)
    {
        return;
    }

    public FixCollider Collider()
    {
        return fixCollider;
    }

    public Collision GetCollision(FixObject other)
    {
        FixCollider collider = other.Collider();

        if (collider == null) return null;

        Collision collision = fixCollider.GetCollision(collider);

        if (collision != null) collision.SetObjectsValues(velocity, IsStatic(), GetPosition());

        return collision;
    }

    public bool IsStatic()
    {
        return true;
    }

    public void Move()
    {
        FixVec3 position = GetPosition();
        angle += FixWorld.deltaTime * speed;
        FixVec3 position2 = GetPosition();
        velocity = (position - position2) * (1 / FixWorld.deltaTime);
        fixCollider.SetPosition(position2);

        transform.position = FixConverter.ToFixVec3(GetPosition());
    }

    public void MoveBackwards()
    {
        FixVec3 position = GetPosition();
        fixCollider.SetPosition(position);
        angle -= FixWorld.deltaTime * speed;
        FixVec3 position2 = GetPosition();
        velocity = (position - position2) * (1 / FixWorld.deltaTime);

        transform.position = FixConverter.ToFixVec3(GetPosition());
    }

    public Collision GetCollision(FixCollider collisions)
    {
        return null;
    }
}
