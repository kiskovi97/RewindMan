using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public delegate void ReactToCollideDelegate(Collision[] collisions);
    public delegate void ReactNotToCollideDelegate();

    [RequireComponent(typeof(FixCollider))]
    public class CollidableObject : MonoBehaviour
    {
        protected FixCollider fixCollider;

        private void Awake()
        {
            fixCollider = GetComponent<FixCollider>();
        }

        // Use this for initialization
        void Start()
        {
            ReactToCollide += DoNothing;
            ReactNotToCollide += DoNothing;
        }

        void DoNothing()
        {

        }

        void DoNothing(Collision[] collisions)
        {

        }

        public void SetPositionAndVelocity(FixVec3 position, FixVec3 velocity)
        {
            fixCollider.SetPositionAndVelocity(position, velocity);
        }

        public Collision GetCollision(FixCollider collider)
        {
            if (collider == null) return null;

            if (collider == fixCollider) return null;

            Collision collision = collider.GetCollision(fixCollider);

            return collision;
        }

        public void Collide(Collision[] collisions)
        {
            if (collisions.Length != 0)
            {
                ReactToCollide(collisions);
            } else
            {
                ReactNotToCollide();
            }
        }

        public event ReactToCollideDelegate ReactToCollide;
        public event ReactNotToCollideDelegate ReactNotToCollide;
    }
}
