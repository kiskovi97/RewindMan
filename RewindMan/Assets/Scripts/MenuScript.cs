using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private static readonly int MENU = 0;
    private static readonly int LEVELSELECTOR = 1;
    private static readonly int LEVEL01 = 2;

    public void Update()
    {
        if (Input.GetAxisRaw("Cancel") != 0)
        {
            Quit();
        }
    }

    public void GoToLevelSelector()
    {
        SceneManager.LoadScene(LEVELSELECTOR);
    }

    public void GoToLevel01()
    {
        SceneManager.LoadScene(LEVEL01);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENU);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
