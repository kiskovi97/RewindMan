using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

[RequireComponent(typeof(FixCollider))]
class RigidObjectOther : RecordedObjectOther
{
    // Inspector Initial values
    public bool isStatic = false;
    public float frictionCoefficientFloat = 0.98f;
    public float impulseLoseCoefficentFloat = 0.5f;

    private static readonly int collideOverlap = 3;
    private static Fix minCollide = new Fix(4);
    private Fix frictionCoefficient;
    private Fix impulseLoseCoefficent;
    private int hasCollided = 0;

    // Inner state 
    private FixCollider fixCollider;
    private FixVec3 savedVelocity = FixVec3.Zero;
    private Forces forces = new Forces();

    protected override void Start()
    {
        base.Start();
        fixCollider = GetComponent<FixCollider>();

        FixVec3 position = (FixConverter.ToFixVec3(transform.position));
        ResetRecording();
        SetPositionAndVelocity(position, Velocity);

        fixCollider.SetPositionAndVelocity(Position, Velocity);
        fixCollider.isStatic = isStatic;
        transform.position = FixConverter.ToFixVec3(Position);

        frictionCoefficient = FixConverter.ToFix(frictionCoefficientFloat);
        impulseLoseCoefficent = FixConverter.ToFix(impulseLoseCoefficentFloat);
        forces = new Forces();
        forces.Clear();
        forces.AddForce(FixWorldComplex.gravity);
        minCollide = FixMath.Abs(FixWorldComplex.gravity.Y) * FixWorldComplex.deltaTime * FixWorldComplex.deltaTime;
    }

    // ---------------- FixObject Implementations -----------------

    public void Move()
    {
        if (isStatic) return;
        Accelerate(forces.GetSumForces());
        Step();
        fixCollider.SetPositionAndVelocity(Position, Velocity);
        forces.Clear();
        hasCollided--;
        if (hasCollided < 0)
        {
            hasCollided = 0;
        }
    }

    public void Collide(Collision[] collisions)
    {
        if (isStatic) return;
        if (collisions.Length != 0)
        {
            hasCollided = collideOverlap;
            ReactToCollide(collisions);
        }
    }

    public Collision GetCollision(FixCollider collider)
    {
        if (collider == null) return null;

        if (collider == fixCollider) return null;

        Collision collision = collider.GetCollision(fixCollider);

        return collision;
    }

    // --------------- Outside 'Forces' ------------

   /* public bool MovePosition(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            VelocityCorrection((Velocity + speed) / 2);
            return true;
        }
        return false;
    }

    public bool AddToSpeed(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            VelocityCorrection(speed);
            hasCollided = 0;
            return true;
        }
        return false;
    }*/

    // --------------- Inner Help Functions  -------------

    void ReactToCollide(Collision[] collisions)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (!collisions[i].isStatic)
            {
                ReactDynamicCollide(collisions[i]);
            }
            OverlapCorrection(collisions[i]);
        }
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].isStatic)
            {
                ReactStaticCollide(collisions[i]);
            }
        }
        startVelocity = FixConverter.ToFixVec3(Velocity);
    }

    private void ReactStaticCollide(Collision collision)
    {
        FixVec3 N = collision.Normal;
        FixVec3 paralellVector = new FixVec3(-N.Y, N.X, N.Z);
        FixVec3 projectedForce = HelpFixMath.Project(Velocity, paralellVector);
        VelocityCorrection(projectedForce * frictionCoefficient);
        forces.AddImpulse(FixWorldComplex.GravitySizeVector(N));
    }

    private void ReactDynamicCollide(Collision collision)
    {
        FixVec3 N = collision.Normal;
        // Direction
        FixVec3 velocity = Velocity + FixMath.Abs(2 * Velocity.Dot(N)) * N;
        // Impulse
        velocity = ((velocity + collision.savedVelocity) / 2) * impulseLoseCoefficent;

        if (Position.Y >= collision.position.Y)
        {
            forces.AddImpulse(FixWorldComplex.GravitySizeVector(N));
        }
        VelocityCorrection(velocity);
    }

    private void OverlapCorrection(Collision collision)
    {
        FixVec3 Something = collision.Overlap;
        Fix length = Something.GetMagnitude();

        if (length == 0) collision.Overlap = collision.Normal;
        if (length > minCollide)
        {
            length -= minCollide;
        }
        else
        {
            length = 0;
        }
        FixVec3 Correction = Something.Normalize() * length;
        if (collision.isStatic)
        {
            if (Position.Y >= collision.position.Y)
            {
                PositionCorrection(Position + Correction);
            }
            else
            {
                PositionCorrection(Position + collision.Overlap);
            }
        }
        else
        {
            if (Position.Y >= collision.position.Y)
            {
                PositionCorrection(Position + Something - Something.Normalize() * minCollide);
            }
        }
    }
}
