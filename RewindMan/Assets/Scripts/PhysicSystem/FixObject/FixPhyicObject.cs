using UnityEngine;
using System.Collections;

public interface FixPhyicObject : FixObject
{
    void Collide(Collision[] collisions);
    void CollideBack(Collision[] collisions);

    Collision GetCollision(FixCollider collisions);
}
