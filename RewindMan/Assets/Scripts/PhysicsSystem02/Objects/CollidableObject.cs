using UnityEngine;
using System.Collections;
namespace FixPhysics
{
    [RequireComponent(typeof(FixCollider))]
    public abstract class CollidableObject : RecordedObject
    {
        protected FixCollider fixCollider;
        public bool isStatic = false;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            fixCollider = GetComponent<FixCollider>();
            fixCollider.SetPositionAndVelocity(state.position, state.velocity);
            fixCollider.isStatic = isStatic;
        }

        public void Collide(Collision[] collisions)
        {
            if (isStatic) return;
            if (collisions.Length != 0)
            {
                ReactToCollide(collisions);
            }
        }

        public Collision GetCollision(FixCollider collider)
        {
            if (collider == null) return null;

            if (collider == fixCollider) return null;

            Collision collision = collider.GetCollision(fixCollider);

            return collision;
        }

        abstract protected void ReactToCollide(Collision[] collisions);
    }
}
