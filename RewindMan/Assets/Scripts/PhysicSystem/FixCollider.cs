using UnityEngine;
using System.Collections;
using FixedPointy;

public class FixCollider : MonoBehaviour
{
    private PhysicalObject obj;
    public float radiusFloat = 0.5f;
    public Fix radius = Fix.Zero;

    private void Start()
    {
        radius = FixConverter.ToFix(radiusFloat);
        obj = GetComponent<PhysicalObject>();
    }

    public FixVec3 ClosestPoint(FixVec3 from)
    {
        FixVec3 direction = from - obj.position;
        return obj.position + direction * radius;
    }

    public void Collide(PhysicalObject other)
    {
        FixVec3 distance = other.position - obj.position;
        if (distance.GetMagnitude() >= (other.collider.radius + radius))
            return;
        obj.velocity *= -1;
    }
}
