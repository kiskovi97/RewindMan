using UnityEngine;
using System.Collections;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class PhysicalObject : MonoBehaviour
{
    new public FixCollider collider;
    public FixVec3 position = FixVec3.Zero;
    public FixVec3 Velocity
    {
        get
        {
            FixTrans3 rotate = FixTrans3.MakeRotationZ(angle);
            return (rotate * FixVec3.UnitX) * speed;
        }
    }
    
    public Fix speed = Fix.Zero;
    private Fix angle = Fix.Zero;
    public int startingSpeed = 1;
    public int startingAngle = 0;

    private void Start()
    {
        collider = GetComponent<FixCollider>();

        position = FixConverter.ToFixVec3(transform.position);
        angle = startingAngle;
        speed = startingSpeed;
    }

    // Update is called once per frame
    public void Move()
    {
        if (PhysicsWorld.forward)
        {
            position += Velocity * PhysicsWorld.deltaTime;
            //position += PhysicsWorld.gravity * PhysicsWorld.deltaTime;
        }
        else if (PhysicsWorld.backward)
        {
            position -= Velocity * PhysicsWorld.deltaTime;
            //position -= PhysicsWorld.gravity * PhysicsWorld.deltaTime;
        }
        transform.position = FixConverter.ToFixVec3(position);
        collider.SetPosition(position);
    }

    public void Collide(PhysicalObject other, bool first = false)
    {
        if (Velocity.GetMagnitude() == 0) return;
        if (collider.Collide(other.collider))
        {
            FixVec3 normal = collider.GetNormal(other.collider);
            DrawVector(normal, Color.red);
            Fix normalAngle = GetAngle(normal);

            if (PhysicsWorld.forward)
            {
                Fix angleChange = normalAngle - (angle - 180);
                angle = normalAngle + (angleChange);

            }
            if (PhysicsWorld.backward)
            {
                Fix angleChange = normalAngle - angle;
                angle = normalAngle + (angleChange) - 180;
            }
            //DrawVector(Velocity, Color.blue);
            //Debug.Log(angle);
           /* if (first)
            {
                Fix tmp = speed;
                speed = other.speed;
                other.speed = tmp;
            }*/
        }
    }

    Fix GetAngle(FixVec3 vector)
    {
        Fix normalAngle = FixMath.Acos(vector.X);
        if (vector.Y < 0) normalAngle = 360 - normalAngle;
        return normalAngle;
    }

    void DrawVector(FixVec3 normal, Color color)
    {
        Vector3 normalFloat = FixConverter.ToFixVec3(normal);
        Debug.DrawLine(transform.position, transform.position + normalFloat, color, 1000f, false);
    }
}
