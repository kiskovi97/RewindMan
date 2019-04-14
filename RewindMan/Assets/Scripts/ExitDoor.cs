using FixPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    SceneTransitions transitions;
    int nextScene = 0;
    private void Start()
    {
        transitions = FindObjectOfType<SceneTransitions>();
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("New Level");
        if (other.tag == "Player")
        {
            FixWorld.Exit();
            transitions.LoadSceneAsync(nextScene);
        }
    }
}

