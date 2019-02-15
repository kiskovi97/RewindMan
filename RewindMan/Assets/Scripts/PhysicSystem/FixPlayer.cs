using UnityEngine;
using FixedPointy;

[RequireComponent(typeof(FixObject))]
public class FixPlayer : MonoBehaviour
{
    private FixObject fixObject;

    public int speed = 20;

    private void Start()
    {
        fixObject = GetComponent<FixObject>();
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
                fixObject.AddToSpeed(new FixVec3(speed * 2, speed * 5, 0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                fixObject.AddToSpeed(new FixVec3(-speed * 2, speed * 5, 0));
            }
            else fixObject.AddToSpeed(new FixVec3(0, speed * 5, 0));
        } else
        {
            if (Input.GetKey(KeyCode.D))
            {
                fixObject.MovePosition(new FixVec3(speed, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                fixObject.MovePosition(new FixVec3(-speed, 0, 0));
            }
        }

    }

    void ByForce()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            fixObject.AddForce(new FixVec3(speed, 0, 0));
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            fixObject.AddForce(new FixVec3(-speed, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            fixObject.AddForce(new FixVec3(-speed, 0, 0));
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            fixObject.AddForce(new FixVec3(speed, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fixObject.AddForce(new FixVec3(0, speed, 0));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            fixObject.AddForce(new FixVec3(0, speed, 0));
        }
        Debug.Log(fixObject.forces.GetSumForces());

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
