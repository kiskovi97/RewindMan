using System;
namespace FixPhysics
{
    public class CollidedState : ICloneable
    {
        public bool collided;
        public CollidedState(bool collidedCount)
        {
            this.collided = collidedCount;
        }
        public virtual object Clone()
        {
            return new CollidedState(this);
        }
        private CollidedState(CollidedState record)
        {
            this.collided = record.collided;
        }
        public static CollidedState RecordFromBase(bool collidedCount)
        {
            return new CollidedState(collidedCount);
        }
    }
}
