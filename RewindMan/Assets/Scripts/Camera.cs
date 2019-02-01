using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform focus;
    public Vector3 distance = new Vector3(0, 5f, -20f);
    public float speed = 0.05f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (focus != null)
        {
            Vector3 go = (focus.position + distance) - transform.position;
            Quaternion next = Quaternion.LookRotation(focus.position - transform.position);
            transform.position += go * speed;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, next, speed);
        }    
    }
}
