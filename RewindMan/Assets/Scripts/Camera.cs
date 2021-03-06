﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{

    public Transform focus;
    public Vector3 distance = new Vector3(0, 5f, -20f);
    public float speed = 0.05f;
    public Vector3 Max = new Vector3(0, 0, 0);

    private float leftMin = 0;
    private float downMin = 0;

    private void Start()
    {
        float Z = distance.z * -1;
        leftMin = Z * Mathf.Tan(Mathf.PI) - Max.x;
        downMin = Max.y - Z * Mathf.Tan(Mathf.PI * 0.85f);
    }

    public void Update()
    {
        if(Input.GetAxisRaw("Cancel") != 0)
        {
            Application.Quit();
        }
        if(Input.GetAxisRaw("Restart") != 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (focus != null)
        {
            Vector3 focusPosition = focus.position;
            if (focusPosition.x < leftMin)
            {
                focusPosition = new Vector3(leftMin, focusPosition.y, focusPosition.z);
            }
            if (focusPosition.y < downMin)
            {
                focusPosition = new Vector3(focusPosition.x, downMin, focusPosition.z);
            }
            Vector3 go = (focusPosition + distance) - transform.position;
            Quaternion next = Quaternion.LookRotation(focusPosition - transform.position);
            float currentSpeed = speed;
            if (go.magnitude < speed) currentSpeed = go.magnitude / 2;
            transform.position += go * speed;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, next, speed);
            //transform.rotation = Quaternion.LookRotation(focusPosition - transform.position, Vector3.up);
            Debug.DrawLine(transform.position, new Vector3(leftMin, downMin, 0), Color.red, Time.deltaTime, false);
        }
    }
}
