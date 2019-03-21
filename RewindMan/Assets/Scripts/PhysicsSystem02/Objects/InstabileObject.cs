using UnityEngine;
using FixedPointy;
using UnityEngine.UI;
using System.Collections;
namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class InstabileObject : RecordedObject
    {
        private CollidableObject collidable;
        public int maxCollide = 15;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            collidable = GetComponent<CollidableObject>();
            collidable.ReactToCollide += Logger;
            state = InstableRecord.RecordFromBase(state,0);
        }

        public override void Move()
        {
            if (((InstableRecord)state).collidedCount > maxCollide) state.velocity += FixVec3.UnitY * -1;
            Step();
            collidable.SetPositionAndVelocity(state.position, state.velocity);
        }

        void Logger(Collision[] collisions)
        {
            foreach(Collision collision in collisions)
            if (collision.tag == "Player")
            {
                    ((InstableRecord)state).collidedCount++;
            }
        }
    }
}
