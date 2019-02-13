using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class PhysicalObject : MonoBehaviour
{
    public FixCollider fixCollider;
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public FixVec3 position = FixVec3.Zero;
    private FixVec3 velocity = FixVec3.Zero;
    private FixVec3 savedVelocity = FixVec3.Zero;
    private PhysicRecording records = new PhysicRecording();
    private Forces forces = new Forces();
    public bool isStatic = false;

    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();
        forces.Clear();
        position = FixConverter.ToFixVec3(transform.position);
        fixCollider.SetPosition(position);
        transform.position = FixConverter.ToFixVec3(position);
        velocity = FixConverter.ToFixVec3(startVelocity);
        records.Add(new PhysicRecording.Record(velocity, PhysicsWorld.time, position));
    }

    public void SetRecord(PhysicRecording.Record record)
    {
        if (record != null)
        {
            velocity = record.velocity;
            position = record.position;
        }
    }

    // Update is called once per frame
    public void Move()
    {
        if (isStatic) return;
        if (PhysicsWorld.forward)
        {
            velocity += forces.GetSumForces() * PhysicsWorld.deltaTime;
            position += velocity * PhysicsWorld.deltaTime;
        }
        else if (PhysicsWorld.backward)
        {
            position -= velocity * PhysicsWorld.deltaTime;
            velocity -= forces.GetSumForces() * PhysicsWorld.deltaTime;
        }
        transform.position = FixConverter.ToFixVec3(position);
        fixCollider.SetPosition(position);
        forces.Clear();
    }

    public bool IsCollided(PhysicalObject other)
    {
        return fixCollider.Collide(other.fixCollider);
    }

    public void CollideAll(PhysicalObject[] collidedeObjects)
    {
        if (isStatic) return;
        savedVelocity = velocity;
        if (collidedeObjects.Length != 0) 
            CollideTwo(collidedeObjects);
        forces.AddImpulse(PhysicsWorld.gravity);
    }

    // velocity = velocity - 2 * velocity.Dot(N) * N - N / 2;

    void CollideTwo(PhysicalObject[] collidedeObjects)
    {
        records.Add(new PhysicRecording.Record(velocity, PhysicsWorld.time, position));
        for (int i = 0; i < collidedeObjects.Length; i++)
        {
            if (!collidedeObjects[i].isStatic)
            {
                FixVec3 N = collidedeObjects[i].fixCollider.GetNormal(fixCollider);
                DrawVector(N, Color.red);
                DrawVector(velocity, Color.blue);
                velocity = velocity + FixMath.Abs(2 * velocity.Dot(N)) * N;
                velocity /= 2;
            }
        }

        for (int i = 0; i < collidedeObjects.Length; i++)
        {
            if (collidedeObjects[i].isStatic)
            {
                FixVec3 N = collidedeObjects[i].fixCollider.GetNormal(fixCollider);
                Fix size = PhysicsWorld.gravity.GetMagnitude();
                FixVec3 paralell = new FixVec3(-N.Y, N.X, N.Z);
                FixVec3 projectForce = HelpFixMath.Project(velocity, paralell);
                DrawVector(velocity, Color.yellow);
                velocity = projectForce * 80 / 81;
                //DrawVector(velocity, Color.green);
                forces.AddImpulse(N.Normalize() * size);
            }
        }

    }

    public void CollideBack()
    {
        SetRecord(records.Get(PhysicsWorld.time));
        forces.AddImpulse(PhysicsWorld.gravity);
    }

    void DrawVector(FixVec3 normal, Color color)
    {
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, Time.fixedDeltaTime, false);
    }
}
