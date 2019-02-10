using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class PhysicalObject : MonoBehaviour
{
    new public FixCollider collider;
    public FixVec3 position = FixVec3.Zero;
    private FixVec3 velocity = FixVec3.Zero;
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    private PhysicRecording records = new PhysicRecording();

    private void Start()
    {
        collider = GetComponent<FixCollider>();

        position = FixConverter.ToFixVec3(transform.position);
        velocity = FixConverter.ToFixVec3(startVelocity);
        records.Add(new PhysicRecording.Record(velocity, PhysicsWorld.time));
    }

    public void SetRecord(PhysicRecording.Record record)
    {
        if (record != null)
            velocity = record.velocity;
    }

    // Update is called once per frame
    public void Move()
    {
        if (PhysicsWorld.forward)
        {
            velocity += PhysicsWorld.gravity * PhysicsWorld.deltaTime;
            position += velocity * PhysicsWorld.deltaTime;
        }
        else if (PhysicsWorld.backward)
        {
            position -= velocity * PhysicsWorld.deltaTime;
            velocity -= PhysicsWorld.gravity * PhysicsWorld.deltaTime;
        }
        transform.position = FixConverter.ToFixVec3(position);
        collider.SetPosition(position);
    }

    public bool IsCollided(PhysicalObject other)
    {
        return collider.Collide(other.collider);
    }

    public void CollideAll(PhysicalObject[] colliders)
    {
        if (colliders.Length == 0) return;
        records.Add(new PhysicRecording.Record(velocity, PhysicsWorld.time));
        FixVec3 N = colliders[0].collider.GetNormal(collider);
        for (int i = 1; i < colliders.Length; i++)
        {
            N += colliders[i].collider.GetNormal(collider);
        }
        N = N.Normalize();
        velocity = velocity - 2 * velocity.Dot(N) * N;
    }

    public void CollideBack()
    {
        SetRecord(records.Get(PhysicsWorld.time));
    }

    //Fix GetAngle(FixVec3 vector)
    //{
    //    Fix normalAngle = FixMath.Acos(vector.X);
    //    if (vector.Y < 0) normalAngle = 360 - normalAngle;
    //    return normalAngle;
    //}

    void DrawVector(FixVec3 normal, Color color)
    {
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, 1000f, false);
    }
}
