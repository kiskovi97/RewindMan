using UnityEngine;
using System.Collections;
using FixedPointy;
using System;

namespace FixPhysics
{
    public class AIState : ICloneable
    {
        public Fix speed;
        public bool dead;
        public AIState(Fix speed, bool dead)
        {
            this.speed = speed;
            this.dead = dead;
        }
        public virtual object Clone()
        {
            return new AIState(this);
        }
        private AIState(AIState record)
        {
            this.speed = record.speed;
            this.dead = record.dead;
        }
        public static AIState RecordFromBase(Fix speed, bool dead)
        {
            return new AIState(speed, dead);
        }
    }
}
