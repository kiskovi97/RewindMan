using UnityEngine;
using FixedPointy;

[RequireComponent(typeof(RigidObjectOther))]
public class FixPlayerOther : MonoBehaviour
{
    public GameObject model;

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
                if (fixObject.AddToSpeed(new FixVec3(speed, speed * 2, 0)))
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                    //fixObject.animator.SetFloat("Speed", 1.0f);
                }

            }
            else if (state.left)
            {
                if (fixObject.AddToSpeed(new FixVec3(-speed, speed * 2, 0)))
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                    //fixObject.animator.SetFloat("Speed", 1.0f);
                }

            }
            else fixObject.AddToSpeed(new FixVec3(0, speed * 2, 0));
        }
        else
        {
            if (state.right)
            {
                if (fixObject.MovePosition(new FixVec3(speed, 0, 0)))
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                    //fixObject.animator.SetFloat("Speed", 1.0f);
                }
            }
            if (state.left)
            {
                if (fixObject.MovePosition(new FixVec3(-speed, 0, 0)))
                {
                    model.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                    //fixObject.animator.SetFloat("Speed", 1.0f);
                }
            }
        }

    }
}
