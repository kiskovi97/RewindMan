﻿using UnityEngine;
using FixedPointy;
using System.Collections.Generic;
namespace FixPhysics
{
    public abstract class MovingObject : RecordedObject<MovingState>
    {
        public Vector3 startVelocity = new Vector3(0, 1, 0);
        private static float maxFallingSpeedIn = 20f;
        public Fix maxFallingSpeed;

        protected virtual void Start()
        {
            FixVec3 Position = FixConverter.ToFixVec3(transform.position);
            FixVec3 Velocity = FixConverter.ToFixVec3(startVelocity);
            state = new MovingState(Velocity, Position);
            maxFallingSpeed = FixConverter.ToFix(maxFallingSpeedIn);
        }

        public override void Update()
        {
            base.Update();
            transform.position = FixConverter.ToFixVec3(state.position);
            startVelocity = FixConverter.ToFixVec3(state.velocity);
        }

        protected void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
        {
            state.position = position;
            state.velocity = velocity;
            if (state.velocity.Y < (maxFallingSpeed * -1)) state.velocity = new FixVec3(state.velocity.X, maxFallingSpeed * -1, state.velocity.Z);
        }

        protected void PositionCorrection(FixVec3 newPosition)
        {
            state.position = newPosition;
        }

        protected void VelocityCorrection(FixVec3 newVelocity)
        {
            state.velocity = newVelocity;
            if (newVelocity.GetMagnitude() < FixWorld.deltaTime * FixWorld.gravity.GetMagnitude()) newVelocity = FixVec3.Zero;
            if (state.velocity.Y < (maxFallingSpeed * -1)) state.velocity = new FixVec3(state.velocity.X, maxFallingSpeed * -1, state.velocity.Z);
        }

        protected void Accelerate(FixVec3 sumForce)
        {
            state.velocity += sumForce * FixWorld.deltaTime;
            if (state.velocity.Y < (maxFallingSpeed * -1)) state.velocity = new FixVec3(state.velocity.X, maxFallingSpeed * -1, state.velocity.Z);
        }

        public abstract void Move();

        public void Step()
        {
            state.position += state.velocity * FixWorld.deltaTime;
        }

        public void Step(FixVec3 velocity)
        {
            state.position += velocity * FixWorld.deltaTime;
        }
    }
}
