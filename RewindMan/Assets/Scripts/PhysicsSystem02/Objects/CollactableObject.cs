using UnityEngine;
using FixedPointy;
using System.Collections;

namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class CollactableObject : MovingObject
    {
        private CollidableObject collidable;
        private new MeshRenderer renderer;
        public int maxCollide = 15;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            collidable = GetComponent<CollidableObject>();
            collidable.ReactToCollide += Logger;
            state = CollactableRecord.RecordFromBase(state, false);
            renderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (((CollactableRecord)state).collided)
            {
                renderer.enabled = false;
                FixPlayer.SetBool(GetInstanceID(), true);
            } else
            {
                FixPlayer.SetBool(GetInstanceID(), false);
                renderer.enabled = true;
            }
        }

        public override void Move()
        {
            if (((CollactableRecord)state).collided) state.velocity += FixVec3.UnitY * -1;
            Step();
            collidable.SetPositionAndVelocity(state.position, state.velocity);
        }

        void Logger(Collision[] collisions)
        {
            foreach (Collision collision in collisions)
                if (collision.tag == "Player")
                {
                    ((CollactableRecord)state).collided = true;
                    FixPlayer.SetBool(GetInstanceID(), true);
                }
        }
    }
}
