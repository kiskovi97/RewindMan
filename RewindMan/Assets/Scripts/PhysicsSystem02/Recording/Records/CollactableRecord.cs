using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    public class CollactableRecord : Record
    {

        public bool collided;
        public CollactableRecord(FixVec3 velocity, Fix time, FixVec3 position, bool collidedCount) : base(velocity, time, position, false)
        {
            this.collided = collidedCount;
        }

        public override Record Copy()
        {
            return new CollactableRecord(this);
        }

        private CollactableRecord(CollactableRecord record) : base(record)
        {
            this.collided = record.collided;
        }

        public static Record RecordFromBase(Record state, bool collidedCount)
        {
            return new CollactableRecord(state.velocity, state.time, state.position, collidedCount);
        }
    }
}
