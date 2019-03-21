using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class MovingRecord : Record
    {
        public Fix angle;
        public MovingRecord(FixVec3 velocity, Fix time, FixVec3 position, Fix angle) : base(velocity, time, position, false)
        {
            this.angle = angle;
        }

        public override Record Copy()
        {
            return new MovingRecord(this);
        }

        private MovingRecord(MovingRecord record) : base(record)
        {
            this.angle = record.angle;
        }

        public static Record RecordFromBase(Record state, Fix angle)
        {
            return new MovingRecord(state.velocity, state.time, state.position, angle);
        }
    }
}
