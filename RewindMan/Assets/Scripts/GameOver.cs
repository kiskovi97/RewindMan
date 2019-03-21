using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPhysics;
using FixPhysicsPrev;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FixWorld.GameOverSet();
            FixWorldComplex.GameOverSet();
        }
    }
}
