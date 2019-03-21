using UnityEngine;
using System.Collections;
using FixedPointy;

namespace FixPhysics
{
    public class ControllableRigidObject : RigidObject
    {

        public bool MovePosition(FixVec3 speed)
        {
            if (OnTheFloor())
            {
                VelocityCorrection((state.velocity + speed) / 2);
                return true;
            }
            return false;
        }

        public bool AddToSpeed(FixVec3 speed)
        {
            if (OnTheFloor())
            {
                VelocityCorrection(speed);
                SetOnTheFloor(false);
                return true;
            }
            return false;
        }
    }
}