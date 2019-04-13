using UnityEngine;
using System.Collections;
using FixedPointy;
using System;

namespace FixPhysics
{
    public class MovingState : ICloneable
    {
        public FixVec3 velocity;
        public FixVec3 position;
        public MovingState(FixVec3 velocity, FixVec3 position)
        {
            this.position = position;
            this.velocity = velocity;
        }
        public MovingState(MovingState state)
        {
            this.position = state.position;
            this.velocity = state.velocity;
        }
        public virtual object Clone()
        {
            return new MovingState(velocity, position);
        }
    }
}
