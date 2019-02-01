using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : RecordedObject
{

    public float speed = 10f;
    public Vector3 distance = new Vector3(10f,0,0);
    private Vector3 APoint;
    private Vector3 BPoint;
    private bool right = true;

    protected override void Start()
    {
        base.Start();
        APoint = transform.position;
        BPoint = transform.position + distance;
        StateChange(Record.State.RIGHT); // TO BPoint
    }

    protected override void ActForWard()
    {
        Vector3 go = new Vector3(0, 0, 0);
        float step = speed * Time.deltaTime;
        Record.State newState = Record.State.STILL;
        if (right)
        {
            if ((BPoint - transform.position).magnitude > step)
            {
                go = distance.normalized * speed * Time.deltaTime;
                newState = Record.State.RIGHT;
            } else
            {
                go = (BPoint - transform.position);
                newState = Record.State.LEFT;
                right = false;
            }
        }
        else
        {
            if ((APoint - transform.position).magnitude > step)
            {
                go = -distance.normalized * speed * Time.deltaTime;
                newState = Record.State.LEFT;
            }
            else
            {
                go = (APoint - transform.position);
                newState = Record.State.RIGHT;
                right = true;
            }
        }
        rigidbody.MovePosition(transform.position + go);
        StateChange(newState);
    }

    protected override void ActBackWard(Record record)
    {
        Vector3 go = new Vector3(0, 0, 0);
        float step = speed * Time.deltaTime;
        state = record.state;
        if (record.state == Record.State.LEFT)
        {
            if ((BPoint - transform.position).magnitude > step)
            {
                go = distance.normalized * speed * Time.deltaTime;
            }
            else
            {
                go = (BPoint - transform.position);
            }
            right = false;
        }
        else if (record.state == Record.State.RIGHT)
        {
            if ((APoint - transform.position).magnitude > step)
            {
                go = -distance.normalized * speed * Time.deltaTime;
            }
            else
            {
                go = (APoint - transform.position);
            }
            right = true;
        }
        rigidbody.MovePosition(transform.position + go);
        base.ActBackWard(record);
    }
}
