using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = GetComponentInParent<GameManager>();
    }
    public void OnRetryBtn()
    {
        SceneManager.LoadScene("MissionMode");
    }

    public void OnMainBtn()
    {

        SceneManager.LoadScene("Start_Scene");
    }

    public void OnExitBtn()
    {
        print("Exit Button Clicked!");
        Application.Quit();
    }

    public void OnResumeButton()
    {
        gm.PauseMenu.SetActive(false);
        Time.timeScale = 1;
        gm.IsPause = false;
    }
}
