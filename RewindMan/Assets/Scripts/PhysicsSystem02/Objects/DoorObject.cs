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
        public new Light[] lights;

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

        public override void Update()
        {
            base.Update();
            if (state.collided)
            {
                foreach(Light light in lights)
                    light.enabled = true;
            } else
            {
                foreach (Light light in lights)
                    light.enabled = false;
            }
            ThreadSafeUpdate();
        }

        public void ThreadSafeUpdate()
        {
            if (state.collided)
            {
                door.Open = true;
            }
            else
            {
                door.Open = false;
            }
        }

        void Collide(Collision[] collisions)
        {
            state.collided = false;
            foreach (Collision collision in collisions)
                if (!collision.isStatic)
                {
                    state.collided = true;
                }
            ThreadSafeUpdate();
        }

        void Free()
        {
            state.collided = false;
            ThreadSafeUpdate();
        }
    }
}
