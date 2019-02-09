using UnityEngine;
using System.Collections;
using FixedPointy;

[RequireComponent(typeof(FixCollider))]
public class PhysicalObject : MonoBehaviour
{
    new public FixCollider collider; 
    public FixVec3 velocity = FixVec3.Zero;
    public Vector3 startingSpeed = new Vector3(1, 0, 0);
    public FixVec3 position;
    private Fix angle = Fix.Zero;

    private void Start()
    {
        position = FixConverter.ToFixVec3(transform.position);
        velocity = FixConverter.ToFixVec3(startingSpeed);
        collider = GetComponent<FixCollider>();
    }

    // Update is called once per frame
    public void Move()
    {
        FixVec3 sumForce = FixVec3.Zero * -1;
        if (PhysicsWorld.forward)
        {
            velocity += sumForce * PhysicsWorld.deltaTime;
            position += velocity * PhysicsWorld.deltaTime;
        }
        else if (PhysicsWorld.backward)
        {
            velocity -= sumForce * PhysicsWorld.deltaTime;
            position -= velocity * PhysicsWorld.deltaTime;
        }
        Fix realAngle = FixConverter.RadToAngle(angle);
        FixVec3 forward = FixVec3.UnitX * FixMath.Cos(realAngle) + FixVec3.UnitY * FixMath.Sin(realAngle);
        transform.rotation = Quaternion.LookRotation(FixConverter.ToFixVec3(forward));
        transform.position = FixConverter.ToFixVec3(position);

    }
}
