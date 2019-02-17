﻿using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class FixObject : MonoBehaviour
{
    private static int collideOverlap = 3;
    private static Fix minCollide = new Fix(4);
    private Fix frictionCoefficient;
    public int hasCollided = 0;

    public int recordsNumber = 0;

    // Initial Values
    public Vector3 startVelocity = new Vector3(0, 1, 0);
    public bool isStatic = false;
    private bool kinematic = false;
    public bool DrawVectors = false;

    // Other Physical Objects Need It
    public FixCollider fixCollider;
    public FixVec3 savedVelocity = FixVec3.Zero;

    // Inner state
    public FixVec3 position = FixVec3.Zero;
    private FixVec3 velocity = FixVec3.Zero;
    public Forces forces = new Forces();

    private FixRecording records = new FixRecording();

    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();

        position = FixConverter.ToFixVec3(transform.position);
        fixCollider.SetPosition(position);
        transform.position = FixConverter.ToFixVec3(position);

        velocity = FixConverter.ToFixVec3(startVelocity);

        records.Add(velocity, FixWorld.time, position, DrawVectors);

        frictionCoefficient = FixConverter.ToFix(0.98f);
        forces = new Forces();
        forces.Clear();
        forces.AddForce(FixWorld.gravity);
        minCollide = FixMath.Abs(FixWorld.gravity.Y) * FixWorld.deltaTime * FixWorld.deltaTime;
    }

    public void MovePosition(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            records.Add(velocity, FixWorld.time, position, DrawVectors);
            position += speed * FixWorld.deltaTime;
            velocity = (velocity + speed) / 2;
        }
    }

    public void AddForce(FixVec3 force)
    {
        forces.AddForce(force);
        records.AddForceChange(force, FixWorld.time);
    }

    public void AddToSpeed(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            records.Add(velocity, FixWorld.time, position, DrawVectors);
            velocity = speed;
            hasCollided = 0;
        }
    }

    public void Move()
    {
        recordsNumber = records.RecordNumber();
        if (isStatic) return;

        position += velocity * FixWorld.deltaTime;
        transform.position = FixConverter.ToFixVec3(position);
        fixCollider.SetPosition(position);

        savedVelocity = velocity;
        velocity += forces.GetSumForces() * FixWorld.deltaTime;

        forces.Clear();
        hasCollided--;
        if (hasCollided < 0) hasCollided = 0;
    }

    public void MoveBackwards()
    {
        if (isStatic) return;
        if (kinematic) return;

        velocity -= forces.GetSumForces() * FixWorld.deltaTime;
        position -= velocity * FixWorld.deltaTime;
        transform.position = FixConverter.ToFixVec3(position);
        fixCollider.SetPosition(position);


        savedVelocity = velocity;
    }

    public Collision GetCollision(FixObject other)
    {
        return other.fixCollider.GetCollision(fixCollider);
    }

    public void Collide(Collision[] collisions)
    {
        if (isStatic) return;
        if (collisions.Length != 0)
        {
            hasCollided = collideOverlap;
            records.Add(velocity, FixWorld.time, position, DrawVectors);
            ReactToCollide(collisions);
        }
    }

    public void CollideBack()
    {
        forces.Clear();
        SetRecord(records.Get(FixWorld.time));
        SetForceRecord(records.GetForceChange(FixWorld.time));

    }

    public void SetRecord(FixRecording.Record record)
    {
        if (record != null)
        {
            velocity = record.velocity;
            position = record.position;
            kinematic = record.kinematic;
        }
    }

    public void SetForceRecord(FixRecording.ForceRecord record)
    {
        if (record != null)
        {
            forces.AddForce(record.force * -1);
        }
    }

    bool zero = false;

    void ReactToCollide(Collision[] collisions)
    {
        zero = false;
        // Non static Collide
        for (int i = 0; i < collisions.Length; i++)
        {
            if (!collisions[i].isStatic && !zero )
            {
                FixVec3 N = collisions[i].Normal;
                // Direction
                velocity = velocity + FixMath.Abs(2 * velocity.Dot(N)) * N;
                // Impulse
                velocity = velocity / 4 + collisions[i].savedVelocity * 3 / 4;

                if (collisions[i].savedVelocity.GetMagnitude() == 0 && position.Y >= collisions[i].position.Y)
                {
                    DrawVector(new FixVec3(0,1,0) * 100, Color.green);
                    velocity = FixVec3.Zero;
                    zero = true;
                } else
                {
                    DrawVector(collisions[i].position - position, Color.green);
                }
            }
            // Position correction for all objects

            FixVec3 Something = collisions[i].Overlap;
            Fix length = Something.GetMagnitude();

            if (length == 0) collisions[i].Overlap = collisions[i].Normal;
            if (length > minCollide)
            {
                length -= minCollide;
                DrawVector(Something.Normalize() * length * 10, Color.green);
            }
            else
            {
                length = 0;
                DrawVector(Something.Normalize() * length * 10, Color.magenta);
            }
            Something = Something.Normalize() * length;
            if (collisions[i].isStatic)
            {
                if (position.Y >= collisions[i].position.Y)
                {
                    position += Something;
                }
                else
                {
                    position += collisions[i].Overlap;
                }
            }
            else
            {
                if (position.Y >= collisions[i].position.Y)
                {
                    position += Something;
                }
            }
            DrawVector(Something, Color.red);
        }

        
        // Static Collide
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].isStatic)
            {
                // Project and fricition calculate
                FixVec3 N = collisions[i].Normal;
                FixVec3 paralellVector = new FixVec3(-N.Y, N.X, N.Z);
                FixVec3 projectedForce = HelpFixMath.Project(velocity, paralellVector);
                velocity = projectedForce * frictionCoefficient;
                forces.AddImpulse(FixWorld.GravitySizeVector(N));
            }
        }

        transform.position = FixConverter.ToFixVec3(position);
        startVelocity = FixConverter.ToFixVec3(velocity);
    }

    void DrawVector(FixVec3 normal, Color color)
    {
        if (!DrawVectors) return;
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, Time.fixedDeltaTime, false);
    }
}
