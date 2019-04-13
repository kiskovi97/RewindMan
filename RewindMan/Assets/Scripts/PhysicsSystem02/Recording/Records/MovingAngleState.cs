using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class MovingAngleState : MovingState
    {
        public Fix angle;
        public MovingAngleState(FixVec3 velocity, FixVec3 position, Fix angle) : base(velocity, position)
        {
            this.angle = angle;
        }
        public override object Clone()
        {
            return new MovingAngleState(this);
        }
        private MovingAngleState(MovingAngleState record) : base(record)
        {
            this.angle = record.angle;
        }
        public static MovingState RecordFromBase(MovingState state, Fix angle)
        {
            return new MovingAngleState(state.velocity, state.position, angle);
        }
    }
}
