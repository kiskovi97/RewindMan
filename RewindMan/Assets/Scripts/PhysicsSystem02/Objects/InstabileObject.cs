using UnityEngine;
using FixedPointy;
using UnityEngine.UI;
using System.Collections;
namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class InstabileObject : MovingObject
    {
        private CollidableObject collidable;
        public int maxCollide = 4;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            collidable = GetComponent<CollidableObject>();
            collidable.ReactToCollide += Logger;
            state = InstableState.RecordFromBase(state,0);
        }

        public override void Move()
        {
            if (((InstableState)state).collidedCount > maxCollide) state.velocity += FixVec3.UnitY * -1;
            Step();
            collidable.SetPositionAndVelocity(state.position, state.velocity);
        }

        void Logger(Collision[] collisions)
        {
            foreach(Collision collision in collisions)
            if (!collision.isStatic)
            {
                    ((InstableState)state).collidedCount++;
            }
        }
    }
}
