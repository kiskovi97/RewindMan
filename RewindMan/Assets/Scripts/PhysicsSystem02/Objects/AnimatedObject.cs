using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class AnimatedObject : MonoBehaviour
    {
        public GameObject model;
        public RigidObject rigidObject;
        public Animator animator;
        // Use this for initialization
        public virtual void Start()
        {
            if (rigidObject == null)
            {
                rigidObject = GetComponent<RigidObject>();
            }
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {
            animator.SetFloat("Time", FixWorld.Backward ? -1 : 1);
            if (rigidObject != null && animator != null && model != null)
            {
                if (rigidObject.OnTheFloor())
                {
                    animator.SetBool("Jump", false);
                }
                else
                {
                    animator.SetBool("Jump", true);
                }

                animator.SetFloat("Speed", Mathf.Abs((float)rigidObject.state.velocity.X) / 3);
                if (rigidObject.state.velocity.X > 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                }
                else if (rigidObject.state.velocity.X < 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                }
            }
        }
    }
}
