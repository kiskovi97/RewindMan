using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

[RequireComponent(typeof(FixCollider))]
class RigidObject : RecordedObject, FixObject
{
    // Inspector Initial values
    public bool isStatic = false;
    public Vector3 startVelocity = new Vector3(0, 1, 0);

    private static int collideOverlap = 3;
    private static Fix minCollide = new Fix(4);
    private Fix frictionCoefficient;
    private int hasCollided = 0;
    
    // Inner state
    private FixCollider fixCollider;
    private FixVec3 savedVelocity = FixVec3.Zero;
    private Forces forces = new Forces();

    private void Start()
    {
        fixCollider = GetComponent<FixCollider>();

        FixVec3 position = (FixConverter.ToFixVec3(transform.position));
        FixVec3 velocity = FixConverter.ToFixVec3(startVelocity);

        SetPositionAndVelocity(position, velocity);

        fixCollider.SetPosition(Position);
        transform.position = FixConverter.ToFixVec3(Position);

        frictionCoefficient = FixConverter.ToFix(0.98f);
        forces = new Forces();
        forces.Clear();
        forces.AddForce(FixWorld.gravity);
        minCollide = FixMath.Abs(FixWorld.gravity.Y) * FixWorld.deltaTime * FixWorld.deltaTime;
    }

    // ---------------- FixObject Implementations -----------------

    public void Move()
    {
        if (isStatic) return;

        Step();
        fixCollider.SetPosition(Position);

        savedVelocity = Velocity;
        Accelerate(forces.GetSumForces());

        forces.Clear();
        hasCollided--;
        if (hasCollided < 0) hasCollided = 0;
    }

    public void MoveBackwards()
    {
        if (isStatic) return;
        if (Kinematic) return;

        AccelerateBack(forces.GetSumForces());
        StepBack();
        fixCollider.SetPosition(Position);


        savedVelocity = Velocity;
    }

    public void Collide(Collision[] collisions)
    {
        if (isStatic) return;
        if (collisions.Length != 0)
        {
            hasCollided = collideOverlap;
            ChangePositionAndVelocity(Position, Velocity);
            ReactToCollide(collisions);
        }
    }

    public void CollideBack(Collision[] collisions)
    {
        if (isStatic) return;
        forces.Clear();
        SetNow();
    }

    public bool IsStatic()
    {
        return isStatic;
    }

    public Collision GetCollision(FixObject other)
    {
        // regen: other.fixCollider.GetCollision(fixCollider)
        FixCollider collider = other.Collider();

        if (collider == null) return null;

        Collision collision = fixCollider.GetCollision(collider);

        if (collision != null) collision.SetObjectsValues(savedVelocity, isStatic, Position);

        return collision;
    }

    public FixCollider Collider()
    {
        return fixCollider;
    }

    // --------------- Outside 'Forces' ------------

    public void MovePosition(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            PositionCorrection(Position + speed * FixWorld.deltaTime);
            VelocityCorrection((Velocity + speed) / 2);
        }
    }

    public void AddToSpeed(FixVec3 speed)
    {
        if (hasCollided > 0)
        {
            VelocityCorrection(speed);
            hasCollided = 0;
        }
    }

    // --------------- Inner Help Functions  -------------

    bool zero = false;

    void ReactToCollide(Collision[] collisions)
    {
        zero = false;
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
        forces.AddImpulse(FixWorld.GravitySizeVector(N));
    }

    private void ReactDynamicCollide(Collision collision)
    {
        if (zero) return;
        FixVec3 N = collision.Normal;
        // Direction
        FixVec3 velocity = Velocity + FixMath.Abs(2 * Velocity.Dot(N)) * N;
        // Impulse
        velocity = velocity / 4 + collision.savedVelocity * 3 / 4;

        if (collision.savedVelocity.GetMagnitude() == 0 && Position.Y >= collision.position.Y)
        {
            DrawVector(new FixVec3(0, 1, 0) * 100, Color.green);
            velocity = FixVec3.Zero;
            zero = true;
        }
        else
        {
            DrawVector(collision.position - Position, Color.green);
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
            DrawVector(Something.Normalize() * length * 10, Color.green);
        }
        else
        {
            length = 0;
            DrawVector(Something.Normalize() * length * 10, Color.magenta);
        }
        Something = Something.Normalize() * length;
        if (collision.isStatic)
        {
            if (Position.Y >= collision.position.Y)
            {
                PositionCorrection(Position + Something);
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
                PositionCorrection(Position + Something);
            }
        }
        DrawVector(Something, Color.red);
    }
}
