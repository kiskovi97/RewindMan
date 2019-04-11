using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    public class PlayerAnimation : AnimatedObject
    {
        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            FixVec3 forceSum = ((RigidRecord)rigidObject.state).forceSum;
            bool push = forceSum.X != 0;
            animator.SetBool("Push", push);
            if (push)
            {
                animator.SetFloat("Speed", Mathf.Abs((float)rigidObject.state.velocity.X) * 2);
                if (forceSum.X > 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                }
                else if (forceSum.X < 0)
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                }
            }
        }
    }
}
