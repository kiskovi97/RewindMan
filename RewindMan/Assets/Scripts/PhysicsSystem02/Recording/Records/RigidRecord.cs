﻿using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class RigidRecord : Record
    {
        public int onTheFloor;
        public FixVec3 prevVelocity = FixVec3.Zero;
        public FixVec3 forceSum = FixVec3.Zero;
        public RigidRecord(FixVec3 velocity, Fix time, FixVec3 position, int collided, FixVec3 prevVelocity, FixVec3 forceSum) : base(velocity, time, position, false)
        {
            this.onTheFloor = collided;
            this.prevVelocity = prevVelocity;
        }

        public override Record Copy()
        {
            return new RigidRecord(this);
        }

        private RigidRecord(RigidRecord record) : base(record)
        {
            this.onTheFloor = record.onTheFloor;
            this.prevVelocity = record.prevVelocity;
            this.forceSum = record.forceSum;
        }

        public static Record RecordFromBase(Record state, int collided, FixVec3 prevVelocity, FixVec3 forceSum)
        {
            return new RigidRecord(state.velocity, state.time, state.position, collided, prevVelocity, forceSum);
        }
    }
}
