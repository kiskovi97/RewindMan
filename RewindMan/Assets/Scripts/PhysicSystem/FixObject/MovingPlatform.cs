﻿using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

namespace FixPhysicsPrev
{
    [RequireComponent(typeof(FixCollider))]
    class MovingPlatform : MonoBehaviour, FixObject
    {
        public Vector3 startDistance = new Vector3(0, 1, 0);
        public float startSpeed = 1.0f;

        private FixCollider fixCollider;
        private Fix speed = Fix.Zero;
        private FixVec3 distance = FixVec3.Zero;
        private FixVec3 start = FixVec3.Zero;
        private FixVec3 velocity = FixVec3.Zero;
        private Fix angle = Fix.Zero;


        private void Start()
        {
            fixCollider = GetComponent<FixCollider>();
            fixCollider.isStatic = true;
            start = (FixConverter.ToFixVec3(transform.position));
            distance = FixConverter.ToFixVec3(startDistance);
            speed = FixConverter.ToFix(startSpeed);
            transform.position = FixConverter.ToFixVec3(GetPosition());
            fixCollider.SetPositionAndVelocity(GetPosition(), velocity);
        }

        private FixVec3 GetPosition()
        {
            return start + distance * (FixMath.Sin(angle * 100) + 1);
        }

        public void Collide(Collision[] collisions)
        {
            return;
        }

        public void CollideBack(Collision[] collisions)
        {
            return;
        }

        public bool IsStatic()
        {
            return true;
        }

        public void Move()
        {
            FixVec3 position = GetPosition();
            angle += FixWorld.deltaTime * speed;
            FixVec3 position2 = GetPosition();
            velocity = (position - position2) * (1 / FixWorld.deltaTime);
            fixCollider.SetPositionAndVelocity(position2, velocity * -1);

            transform.position = FixConverter.ToFixVec3(GetPosition());
        }

        public void MoveBackwards()
        {
            FixVec3 position = GetPosition();
            fixCollider.SetPositionAndVelocity(position, velocity);
            angle -= FixWorld.deltaTime * speed;
            FixVec3 position2 = GetPosition();
            velocity = (position - position2) * (1 / FixWorld.deltaTime);

            transform.position = FixConverter.ToFixVec3(GetPosition());
        }

        public Collision GetCollision(FixCollider collisions)
        {
            return null;
        }
    }
}
