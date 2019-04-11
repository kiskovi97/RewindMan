using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    private Vector3 basePos;
    public float size = 2.0f;
    private float angle = 0.0f;
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * speed;
        transform.position = getPostion(angle);
    }

    Vector3 getPostion(float angle)
    {
        return basePos + Mathf.Cos(angle) * Vector3.left + Mathf.Sin(angle * 3) / 3 * Vector3.down;
    }
}
