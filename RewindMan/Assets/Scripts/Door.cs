using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool open;
    public bool Open
    {
        get { return open; }

        set {
            open = value;
            animator.SetBool("Open", open);
            collider.Enabled = !open;
        }
    }
    Animator animator;
    new FixCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<FixCollider>();
    }
}
