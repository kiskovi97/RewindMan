using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPhysics;
using FixPhysicsPrev;

public class GameOver : MonoBehaviour
{
    public bool IsOn
    {
        set; private get;
    }

    private void Start()
    {
        IsOn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOn) return;
        if (other.tag == "Player")
        {
            FixPhysicsPrev.FixWorld.GameOverSet();
            FixPhysics.FixWorld.GameOverSet();
        }
    }
}
