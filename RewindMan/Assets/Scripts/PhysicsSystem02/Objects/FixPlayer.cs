using UnityEngine;
using UnityEngine.UI;
using FixedPointy;
using FixPhysics;
using System.Collections.Generic;

namespace FixPhysics
{
    [RequireComponent(typeof(ControllableRigidObject))]
    public class FixPlayer : MonoBehaviour
    {
        private ControllableRigidObject fixObject;
        public Text text;
        private static Dictionary<int, bool> dictionary = new Dictionary<int, bool>();

        public static void SetBool(int id, bool set)
        {

            dictionary[id] = set;
        }

        public int speed = 20;

        private void Start()
        {
            fixObject = GetComponent<ControllableRigidObject>();
        }

        private void Update()
        {
            int collected = 0; 
            foreach (bool set in dictionary.Values)
            {
                if (set) collected++;
            }
            text.text = collected + "";
        }

        public void KeyCheck(InputRecord state)
        {
            ByMovePosition(state);
        }

        void ByMovePosition(InputRecord state)
        {
            //fixObject.animator.SetFloat("Speed", 0.0f);
            if (state.up)
            {
                if (state.right)
                {
                    fixObject.Jump(new FixVec3(0, speed * 2, 0));
                    fixObject.AddToSpeed(new FixVec3(speed, 0, 0));
                }
                else if (state.left)
                {
                    fixObject.Jump(new FixVec3(0, speed * 2, 0));
                    fixObject.AddToSpeed(new FixVec3(-speed, 0, 0));
                }
                else fixObject.Jump(new FixVec3(0, speed * 2, 0));
            }
            else
            {
                if (state.right)
                {
                    fixObject.MovePosition(new FixVec3(speed, 0, 0));
                }
                if (state.left)
                {
                    fixObject.MovePosition(new FixVec3(-speed, 0, 0));
                }
            }

        }
    }
}
