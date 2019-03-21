using UnityEngine;
using FixedPointy;
namespace FixPhysicsPrev
{
    [RequireComponent(typeof(RigidObject))]
    public class FixPlayer : MonoBehaviour
    {
        public GameObject model;

        private RigidObject fixObject;

        public int speed = 20;

        private void Start()
        {
            fixObject = GetComponent<RigidObject>();
            fixObject.animator.speed = speed / 5;
        }

        public void KeyCheck()
        {
            ByMovePosition();
        }

        void ByMovePosition()
        {
            fixObject.animator.SetFloat("Speed", 0.0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    if (fixObject.AddToSpeed(new FixVec3(speed, speed * 2, 0)))
                    {
                        model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                        fixObject.animator.SetFloat("Speed", 1.0f);
                    }

                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (fixObject.AddToSpeed(new FixVec3(-speed, speed * 2, 0)))
                    {
                        model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                        fixObject.animator.SetFloat("Speed", 1.0f);
                    }

                }
                else fixObject.AddToSpeed(new FixVec3(0, speed * 2, 0));
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    if (fixObject.MovePosition(new FixVec3(speed, 0, 0)))
                    {
                        model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                        fixObject.animator.SetFloat("Speed", 1.0f);
                    }
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if (fixObject.MovePosition(new FixVec3(-speed, 0, 0)))
                    {
                        model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                        fixObject.animator.SetFloat("Speed", 1.0f);
                    }
                }
            }

        }

        void BySpeed()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                fixObject.AddToSpeed(new FixVec3(speed, 0, 0));
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                fixObject.AddToSpeed(new FixVec3(-speed, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                fixObject.AddToSpeed(new FixVec3(-speed, 0, 0));
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                fixObject.AddToSpeed(new FixVec3(speed, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fixObject.AddToSpeed(new FixVec3(0, 3 * speed, 0));
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                fixObject.AddToSpeed(new FixVec3(0, -3 * speed, 0));
            }
        }
    }
}
