using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public interface FixObject 
{
    void Move();
    void MoveBackwards();

    void Collide(Collision[] collisions);
    void CollideBack(Collision[] collisions);

    bool IsStatic();

    Collision GetCollision(FixObject collisions);
    
    FixCollider Collider();
}
