using System;
namespace FixPhysics
{
    public class CollidedCountState : ICloneable
    {
        public int collidedCount;
        public CollidedCountState(int collidedCount)
        {
            this.collidedCount = collidedCount;
        }
        public virtual object Clone()
        {
            return new CollidedCountState(this);
        }
        private CollidedCountState(CollidedCountState record)
        {
            this.collidedCount = record.collidedCount;
        }
    }
}
