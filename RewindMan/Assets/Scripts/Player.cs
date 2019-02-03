using UnityEngine;
using System.Collections;

public class Player : RecordedObject
{
    public float speed = 10f;
    private float distToGround = 0.5f; 
    private bool IsGrounded()  {
         return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    protected override void ActForWard()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidbody.AddForce(new Vector3(0, 1.5f, 0)*speed, ForceMode.Impulse);
        } else if (Input.GetKey(KeyCode.A))
        {
            rigidbody.MovePosition(transform.position + new Vector3(-1f, 0, 0) * Time.deltaTime * speed);
            StateChange(Record.State.LEFT);
        } else if (Input.GetKey(KeyCode.D))
        {
            rigidbody.MovePosition(transform.position + new Vector3(1f, 0, 0) * Time.deltaTime * speed);
            StateChange(Record.State.RIGHT);
        } else StateChange(Record.State.STILL);
    }

    protected override void ActBackWard(Record record)
    {
        if (state == Record.State.LEFT)
        {
            rigidbody.MovePosition(transform.position + new Vector3(1f, 0, 0) * Time.deltaTime * speed);
        }
        if (state == Record.State.RIGHT)
        {
            rigidbody.MovePosition(transform.position + new Vector3(-1f, 0, 0) * Time.deltaTime * speed);
        }
        if (state == Record.State.JUMPED)
        {
            transform.position = record.position;
            transform.rotation = record.rotation;
        }
        base.ActBackWard(record);
    }
}
