using UnityEngine;
using System.Collections;
namespace FixPhysicsPrev
{
    public interface FixPhyicObject : FixObject
    {
        void Collide(Collision[] collisions);
        void CollideBack(Collision[] collisions);

        Collision GetCollision(FixCollider collisions);
    }
}
