using System;
namespace FixPhysics
{
    public class InputState : ICloneable
    {
        public bool left;
        public bool right;
        public bool up;
        public InputState(bool left, bool right, bool up)
        {
            this.right = right;
            this.left = left;
            this.up = up;
        }
        public InputState()
        {
            this.right = false;
            this.left = false;
            this.up = false;
        }
        public object Clone()
        {
            return new InputState(left, right, up);
        }
    }
}
