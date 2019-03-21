using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

[RequireComponent(typeof(FixCollider))]
public class RigidObjectOther : RecordedObjectOther
{
    // Inspector Initial values
    public bool isStatic = false;
    public float frictionCoefficientFloat = 0.98f;
    public float impulseLoseCoefficentFloat = 0.5f;

    private static readonly int collideOverlap = 1;
    private static Fix minCollide = new Fix(4);
    private Fix frictionCoefficient;
    private Fix impulseLoseCoefficent;

    // Inner state 
    private FixCollider fixCollider;
    private FixVec3 savedVelocity = FixVec3.Zero;
    private Forces forces = new Forces();

    protected override void Start()
    {
        base.Start();
        fixCollider = GetComponent<FixCollider>();
        ResetRecording();

        state = RigidRecord.RecordFromBase(state, 0, FixVec3.Zero);

        fixCollider.SetPositionAndVelocity(state.position, state.velocity);
        fixCollider.isStatic = isStatic;

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
        Step(((RigidRecord)state).prevVelocity);
        ((RigidRecord)state).prevVelocity = FixVec3.Zero;
        fixCollider.SetPositionAndVelocity(state.position, state.velocity);
        forces.Clear();
        SetOnTheFloor(false);
    }

    public void Collide(Collision[] collisions)
    {
        if (isStatic) return;
        if (collisions.Length != 0)
        {
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

   public bool MovePosition(FixVec3 speed)
    {
        if (OnTheFloor())
        {
            VelocityCorrection((state.velocity + speed) / 2);
            return true;
        }
        return false;
    }

    public bool AddToSpeed(FixVec3 speed)
    {
        if (OnTheFloor())
        {
            VelocityCorrection(speed);
            SetOnTheFloor(false);
            return true;
        }
        return false;
    }

    public bool OnTheFloor()
    {
        return ((RigidRecord)state).onTheFloor > 0;
    }

    // --------------- Inner Help Functions  -------------

    private void SetOnTheFloor(bool collided)
    {
        if (collided)
        {
            ((RigidRecord)state).onTheFloor = collideOverlap;
        }
        else
        {
            RigidRecord fullState = ((RigidRecord)state);
            fullState.onTheFloor--;
            if (fullState.onTheFloor < 0) fullState.onTheFloor = 0;
            state = fullState;
        }
    }

    void ReactToCollide(Collision[] collisions)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (!collisions[i].isStatic)
            {
                ReactDynamicCollide(collisions[i]);
            }
            OverlapCorrection(collisions[i]);
            if (collisions[i].Normal.Y > 0)
            {
                SetOnTheFloor(true);
            }
        }
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].isStatic)
            {
                ReactStaticCollide(collisions[i]);
            }
        }
        startVelocity = FixConverter.ToFixVec3(state.velocity);
    }

    private void ReactStaticCollide(Collision collision)
    {
        FixVec3 N = collision.Normal;
        FixVec3 paralellVector = new FixVec3(-N.Y, N.X, N.Z);
        FixVec3 projectedForce = HelpFixMath.Project(state.velocity, paralellVector);
        VelocityCorrection(projectedForce * frictionCoefficient);

        ((RigidRecord)state).prevVelocity += collision.savedVelocity;
    }

    private void ReactDynamicCollide(Collision collision)
    {
        FixVec3 N = collision.Normal;
        // Direction
        FixVec3 velocity = state.velocity + FixMath.Abs(2 * state.velocity.Dot(N)) * N;
        // Impulse
        velocity = ((velocity + collision.savedVelocity) / 2) * impulseLoseCoefficent;

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
            if (state.position.Y >= collision.position.Y)
            {
                PositionCorrection(state.position + Correction);
            }
            else
            {
                PositionCorrection(state.position + collision.Overlap);
            }
        }
        else
        {
            if (state.position.Y >= collision.position.Y)
            {
                PositionCorrection(state.position + Something - Something.Normalize() * minCollide);
            }
        }
    }
}
