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
            fixObject.AddForce(new FixVec3(0,  3 * speed, 0));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            fixObject.AddForce(new FixVec3(0, -3 * speed, 0));
        }
        Debug.Log(fixObject.forces.GetSumForces());
    }
}
