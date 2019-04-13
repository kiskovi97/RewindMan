using UnityEngine;
using FixedPointy;
using System.Collections;

namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class CollactableObject : RecordedObject<CollidedState>
    {
        private CollidableObject collidable;
        private new MeshRenderer renderer;
        public int maxCollide = 15;

        // Use this for initialization
        protected virtual void Start()
        {
            collidable = GetComponent<CollidableObject>();
            collidable.ReactToCollide += Logger;
            state = new CollidedState(false);
            renderer = GetComponent<MeshRenderer>();
        }

        public override void Update()
        {
            base.Update();
            if (state.collided)
            {
                renderer.enabled = false;
                FixPlayer.SetBool(GetInstanceID(), true);
            } else
            {
                FixPlayer.SetBool(GetInstanceID(), false);
                renderer.enabled = true;
            }
        }

        void Logger(Collision[] collisions)
        {
            foreach (Collision collision in collisions)
                if (collision.tag == "Player")
                {
                    state.collided = true;
                    FixPlayer.SetBool(GetInstanceID(), true);
                }
        }
    }
}
