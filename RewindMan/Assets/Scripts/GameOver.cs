using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GameOver : MonoBehaviour
{
    private bool gameOver = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            gameOver = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 2;
            gameOver = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) Time.timeScale = 2;
        else if (!gameOver) Time.timeScale = 1;
    }
}
