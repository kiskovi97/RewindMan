using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class FixObject : MonoBehaviour
{
    private Fix frictionCoefficient;
    private bool hasCollided = false;

    // Initial Values
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public bool isStatic = false;
    public bool DrawVectors = false;

    // Other Physical Objects Need It
    public FixCollider fixCollider;
    private FixVec3 savedVelocity = FixVec3.Zero;

    // Inner state
    private FixVec3 position = FixVec3.Zero;
    private FixVec3 velocity = FixVec3.Zero;
    public Forces forces = new Forces();

    private PhysicRecording records = new PhysicRecording();

    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();

        position = FixConverter.ToFixVec3(transform.position);
        fixCollider.SetPosition(position);
        transform.position = FixConverter.ToFixVec3(position);

        velocity = FixConverter.ToFixVec3(startVelocity);

        records.Add(velocity, FixWorld.time, position);

        frictionCoefficient = FixConverter.ToFix(0.98f);
        forces = new Forces();
        forces.Clear();
        forces.AddForce(FixWorld.gravity);
    }

    public void MovePosition(FixVec3 speed)
    {
        if (hasCollided)
        {
            position += speed * FixWorld.deltaTime;
        }
    }

    public void AddForce(FixVec3 force)
    {
        forces.AddForce(force);
        records.AddForceChange(force, FixWorld.time);
    }

    public void AddToSpeed(FixVec3 speed)
    {
        if (hasCollided)
        {
            records.Add(velocity, FixWorld.time, position);
            velocity += speed;
        }
    }

    public void Move()
    {
        if (isStatic) return;

        velocity += forces.GetSumForces() * FixWorld.deltaTime;
        DrawVector(forces.GetSumForces(), Color.blue);
        DrawVector(velocity, Color.green);
        position += velocity * FixWorld.deltaTime;
        transform.position = FixConverter.ToFixVec3(position);
        fixCollider.SetPosition(position);

        forces.Clear();
        savedVelocity = velocity;
        hasCollided = false;
    }

    public void MoveBackwards()
    {
        if (isStatic) return;

        position -= velocity * FixWorld.deltaTime;
        transform.position = FixConverter.ToFixVec3(position);
        fixCollider.SetPosition(position);

        velocity -= forces.GetSumForces() * FixWorld.deltaTime;

        forces.Clear();
        savedVelocity = velocity;
    }

    public bool IsCollided(FixObject other)
    {
        return fixCollider.Collide(other.fixCollider);
    }

    public void Collide(FixObject[] collidedeObjects)
    {
        if (isStatic) return;
        if (collidedeObjects.Length != 0)
        {
            hasCollided = true;
            records.Add(velocity, FixWorld.time, position);
            ReactToCollide(collidedeObjects);
        }
    }

    public void CollideBack()
    {
        SetRecord(records.Get(FixWorld.time));
        SetForceRecord(records.GetForceChange(FixWorld.time));

    }

    public void SetRecord(PhysicRecording.Record record)
    {
        if (record != null)
        {
            velocity = record.velocity;
            position = record.position;
        }
    }

    public void SetForceRecord(PhysicRecording.ForceRecord record)
    {
        if (record != null)
        {
            forces.AddForce(record.force * - 1);
        }
    }

    void ReactToCollide(FixObject[] collidedeObjects)
    {
        // Non static Collide
        for (int i = 0; i < collidedeObjects.Length; i++)
        {
            if (!collidedeObjects[i].isStatic)
            {
                FixVec3 N = collidedeObjects[i].fixCollider.GetNormal(fixCollider);
                // Direction
                velocity = velocity + FixMath.Abs(2 * velocity.Dot(N)) * N;
                // Impulse
                velocity = velocity / 2 + collidedeObjects[i].savedVelocity / 2;
            }
            // Position correction for all objects
            FixVec3 Something = collidedeObjects[i].fixCollider.GetIntersection(fixCollider);
            if (collidedeObjects[i].isStatic)
            {
                position += Something * 4 / 5;
            } else
            {
                position += Something * 1 / 2;
            }
            DrawVector(collidedeObjects[i].position - position, Color.red);
        }

        // Static Collide
        for (int i = 0; i < collidedeObjects.Length; i++)
        {
            if (collidedeObjects[i].isStatic)
            {
                // Project and fricition calculate
                FixVec3 N = collidedeObjects[i].fixCollider.GetNormal(fixCollider);
                FixVec3 paralellVector = new FixVec3(-N.Y, N.X, N.Z);
                FixVec3 projectedForce = HelpFixMath.Project(velocity, paralellVector);
                velocity = projectedForce * frictionCoefficient;
                forces.AddImpulse(FixWorld.GravitySizeVector(N));
            }
        }

    }

    void DrawVector(FixVec3 normal, Color color)
    {
        if (!DrawVectors) return;
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, Time.fixedDeltaTime, false);
    }
}
