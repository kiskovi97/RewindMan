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
            if (state.collided)
            {
                door.Open = true;
                foreach(Light light in lights)
                    light.enabled = true;
            } else
            {
                door.Open = false;
                foreach (Light light in lights)
                    light.enabled = false;
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
            Update();
        }

        void Free()
        {
            state.collided = false;
            Update();
        }
    }
}
