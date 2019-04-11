using UnityEngine;
using FixedPointy;
using System.Collections;

namespace FixPhysics
{
    public class DoorObject : MovingObject
    {
        public CollidableObject collidable;
        private new MeshRenderer renderer;
        public Door door;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            if (collidable == null)
            {
                collidable = GetComponent<CollidableObject>();
            }
            collidable.ReactToCollide += Collide;
            collidable.ReactNotToCollide += Free;
            state = CollactableRecord.RecordFromBase(state, false);
            renderer = GetComponent<MeshRenderer>();
        }

        void Collide(Collision[] collisions)
        {
            door.Open = false;
            foreach (Collision collision in collisions)
                if (!collision.isStatic)
                {
                    ((CollactableRecord)state).collided = true;
                    door.Open = true;
                }
        }

        void Free()
        {
            door.Open = false;
        }

        public override void Move()
        {
        }
    }
}
