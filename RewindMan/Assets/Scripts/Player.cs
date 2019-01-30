using UnityEngine;
using System.Collections;

public class Player : MovingObject
{
    public float speed = 10f;

    protected override void ActForWard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(new Vector3(0, 3f, 0)*speed, ForceMode.Impulse);
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
