using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RecordedObject : MonoBehaviour
{
    new protected Rigidbody rigidbody;
    protected Record.State state;
    protected float time = 0f;
    private Recording recording = new Recording();
    private bool initKinematic = false;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        initKinematic = rigidbody.isKinematic;
        state = Record.State.STILL;
        time = 0f;
        recording.Add(new Record(-0.1f, state, transform.position, transform.rotation));
    }
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q)) BackWard();
        else Forward();
    }

    void Forward()
    {
        rigidbody.isKinematic = initKinematic;
        time += Time.deltaTime;
        ActForWard();
        SimulatedCheck();
    }

    void BackWard()
    {
        rigidbody.isKinematic = true;
        Record record = recording.Get(time);
        state = record.state;
        ActBackWard(record);
        time -= Time.deltaTime;
        if (time < 0f) time = 0f;
    }
    
    protected void StateChange(Record.State state)
    {
        if (state != this.state || state == Record.State.SIMULATED)
        {
            recording.Add(new Record(time, state, transform.position, transform.rotation));
        }
        this.state = state;
    }

    private void SimulatedCheck()
    {
        if (!initKinematic && rigidbody.velocity.sqrMagnitude > 0.001f)
        {
            StateChange(Record.State.SIMULATED);
        }
    }

    protected virtual void ActForWard() { StateChange(Record.State.STILL); }

    protected virtual void ActBackWard(Record record)
    {
        if (state == Record.State.SIMULATED || state == Record.State.STILL)
        {
            transform.position = record.position;
            transform.rotation = record.rotation;
        }
    }
}
