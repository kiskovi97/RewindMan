using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class RigidState: MovingState
    {
        public int onTheFloor;
        public FixVec3 prevVelocity = FixVec3.Zero;
        public FixVec3 forceSum = FixVec3.Zero;
        public RigidState(FixVec3 velocity, FixVec3 position, int collided, FixVec3 prevVelocity, FixVec3 forceSum) : base(velocity, position)
        {
            this.onTheFloor = collided;
            this.prevVelocity = prevVelocity;
            this.forceSum = forceSum;
        }
        public override object Clone()
        {
            return new RigidState(this);
        }
        private RigidState(RigidState record) : base(record)
        {
            this.onTheFloor = record.onTheFloor;
            this.prevVelocity = record.prevVelocity;
            this.forceSum = record.forceSum;
        }
        public static MovingState RecordFromBase(MovingState state, int collided, FixVec3 prevVelocity, FixVec3 forceSum)
        {
            return new RigidState(state.velocity, state.position, collided, prevVelocity, forceSum);
        }
    }
}
