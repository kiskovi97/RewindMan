using UnityEngine;
using System.Collections;
using FixedPointy;

public class FixCollider : MonoBehaviour
{
    private FixVec3 position;
    public float radiusFloat = 0.5f;
    public Fix radius = Fix.Zero;

    private void Start()
    {
        radius = FixConverter.ToFix(radiusFloat);
    }

    public void SetPosition(FixVec3 position)
    {
        this.position = position;
    }

    public bool Collide(FixCollider other)
    {
        FixVec3 distance = other.position - position;
        if (distance.GetMagnitude() >= (other.radius + radius))
            return false;
       return true;
    }

    public FixVec3 GetNormal(FixCollider other)
    {
        return (position - other.position).Normalize();
    }
}
