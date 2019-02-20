using UnityEngine;
using FixedPointy;

[RequireComponent(typeof(RigidObject))]
public class FixPlayer : MonoBehaviour
{
    private RigidObject fixObject;

    public int speed = 20;

    private void Start()
    {
        fixObject = GetComponent<RigidObject>();
    }

    public void KeyCheck()
    {
        ByMovePosition();
    }

    void ByMovePosition()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.D))
            {
                fixObject.AddToSpeed(new FixVec3(speed * 2, speed * 3, 0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                fixObject.AddToSpeed(new FixVec3(-speed * 2, speed * 3, 0));
            }
            else fixObject.AddToSpeed(new FixVec3(0, speed * 2, 0));
        } else
        {
            if (Input.GetKey(KeyCode.D))
            {
                fixObject.MovePosition(new FixVec3(speed * 2, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                fixObject.MovePosition(new FixVec3(-speed * 2, 0, 0));
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
