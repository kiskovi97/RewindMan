using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GameOver : MonoBehaviour
{
    private float BackwardTime = 1f;
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
            Time.timeScale = BackwardTime;
            gameOver = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) Time.timeScale = BackwardTime;
        else if (!gameOver) Time.timeScale = 1;
    }
}
