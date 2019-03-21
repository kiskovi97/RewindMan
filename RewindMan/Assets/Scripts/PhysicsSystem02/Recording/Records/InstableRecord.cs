using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    public class InstableRecord : Record
    {

        public int collidedCount;
        public InstableRecord(FixVec3 velocity, Fix time, FixVec3 position, int collidedCount) : base(velocity, time, position, false)
        {
            this.collidedCount = collidedCount;
        }

        public override Record Copy()
        {
            return new InstableRecord(this);
        }

        private InstableRecord(InstableRecord record) : base(record)
        {
            this.collidedCount = record.collidedCount;
        }

        public static Record RecordFromBase(Record state, int collidedCount)
        {
            return new InstableRecord(state.velocity, state.time, state.position, collidedCount);
        }
    }
}
