using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    public class InstableState : MovingState
    {
        public int collidedCount;
        public InstableState(FixVec3 velocity, FixVec3 position, int collidedCount) : base(velocity, position)
        {
            this.collidedCount = collidedCount;
        }
        public override object Clone()
        {
            return new InstableState(this);
        }
        private InstableState(InstableState record) : base(record)
        {
            this.collidedCount = record.collidedCount;
        }
        public static InstableState RecordFromBase(MovingState state, int collidedCount)
        {
            return new InstableState(state.velocity, state.position, collidedCount);
        }
    }
}
