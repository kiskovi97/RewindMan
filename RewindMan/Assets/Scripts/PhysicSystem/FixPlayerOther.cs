using UnityEngine;
using FixedPointy;

[RequireComponent(typeof(RigidObjectOther))]
public class FixPlayerOther : MonoBehaviour
{
    private RigidObjectOther fixObject;

    public int speed = 20;

    private void Start()
    {
        fixObject = GetComponent<RigidObjectOther>();
        //fixObject.animator.speed = speed / 5;
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
                fixObject.AddToSpeed(new FixVec3(speed, speed * 2, 0));
            }
            else if (state.left)
            {
                fixObject.AddToSpeed(new FixVec3(-speed, speed * 2, 0));

            }
            else fixObject.AddToSpeed(new FixVec3(0, speed * 2, 0));
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
