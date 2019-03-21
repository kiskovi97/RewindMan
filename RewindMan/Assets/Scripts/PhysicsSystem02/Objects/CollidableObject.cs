using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public delegate void ReactToCollideDelegate(Collision[] collisions);

    [RequireComponent(typeof(FixCollider))]
    public class CollidableObject : MonoBehaviour
    {
        protected FixCollider fixCollider;

        // Use this for initialization
        void Start()
        {
            fixCollider = GetComponent<FixCollider>();
        }

        public void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
        {
            fixCollider.SetPositionAndVelocity(position, velocity);
        }

        public void Collide(Collision[] collisions)
        {
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

        public event ReactToCollideDelegate ReactToCollide;
    }
}
