using UnityEngine;
using System.Collections;

public class Record
{
    public enum State
    {
        STILL, SIMULATED, JUMPED, LEFT, RIGHT
    };

    public float time;
    public State state;
    public Vector3 position;
    public Quaternion rotation;
    public Record(float time, State state, Vector3 position, Quaternion rotation)
    {
        this.time = time;
        this.state = state;
        this.position = position;
        this.rotation = rotation;
    }
}
