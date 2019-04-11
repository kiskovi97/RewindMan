using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    public class PlayerAnimation : AnimatedObject
    {
        private CollidableObject collidableObject;
        private bool push = false;
        private FixVec3 normal = FixVec3.Zero;

        public override void Start()
        {
            base.Start();
            collidableObject = GetComponent<CollidableObject>();
            collidableObject.ReactToCollide += Collide;
            collidableObject.ReactNotToCollide += NoCollide;

        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            animator.SetBool("Push", push);
            if (push)
            {
                animator.SetFloat("Speed", Mathf.Abs((float)rigidObject.state.velocity.X) * 2);
                if (normal.X > 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                }
                else if (normal.X < 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                }
            }
        }

        void Collide(Collision[] collisions)
        {
            push = false;
            foreach(Collision collision in collisions)
            {
                if (collision.Normal.X != 0)
                {
                    push = true;
                    normal = collision.Normal;
                }
            }
        }

        void NoCollide()
        {
            push = false;
            normal = FixVec3.Zero;
        }
    }
}
