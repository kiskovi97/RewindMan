using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAnim;
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
        StartCoroutine("LoadScene", LEVELSELECTOR);
    }

    public void GoToLevel01()
    {
        StartCoroutine("LoadScene", LEVEL01);
    }

    public void GoToMenu()
    {
        StartCoroutine("LoadScene", MENU);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadSceneAsync(int number)
    {
        StartCoroutine("LoadScene", number);
    }

    IEnumerator LoadScene(int sceneNumber)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneNumber);
    }
}
