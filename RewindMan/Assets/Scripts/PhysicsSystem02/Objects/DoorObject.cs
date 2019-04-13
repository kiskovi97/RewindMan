using UnityEngine;
using FixedPointy;
using System.Collections;

namespace FixPhysics
{
    public class DoorObject : RecordedObject<CollidedState>
    {
        public CollidableObject collidable;
        private new MeshRenderer renderer;
        public Door door;

        // Use this for initialization
        protected virtual void Start()
        {
            if (collidable == null)
            {
                collidable = GetComponent<CollidableObject>();
            }
            collidable.ReactToCollide += Collide;
            collidable.ReactNotToCollide += Free;
            state = new CollidedState(false);
            renderer = GetComponent<MeshRenderer>();
        }

        void Collide(Collision[] collisions)
        {
            door.Open = false;
            foreach (Collision collision in collisions)
                if (!collision.isStatic)
                {
                    state.collided = true;
                    door.Open = true;
                }
        }

        void Free()
        {
            door.Open = false;
        }
    }
}
