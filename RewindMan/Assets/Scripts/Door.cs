using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Open = false;
    Animator animator;
    new FixCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<FixCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Open", Open);
        collider.Enabled = !Open;
    }
}
