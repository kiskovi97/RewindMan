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
            Fix addedSpeed = (speed.X) / 5 + state.velocity.X;
            Fix XSpeed = FixMath.Abs(addedSpeed) < FixMath.Abs(speed.X) ? addedSpeed : speed.X;
            FixVec3 relativeSpeed = new FixVec3(XSpeed, state.velocity.Y, 0);
            VelocityCorrection(relativeSpeed);
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
            Fix addedSpeed = (speed.X) / 5 + state.velocity.X;
            Fix XSpeed = FixMath.Abs(addedSpeed) < FixMath.Abs(speed.X) ? addedSpeed : speed.X;
            FixVec3 relativeSpeed = new FixVec3(XSpeed, state.velocity.Y, 0);
            VelocityCorrection(relativeSpeed);
            return false;
        }
    }
}