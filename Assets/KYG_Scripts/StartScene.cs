using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartScene : MonoBehaviour
{
    public GameObject tutorialBtn;
    public GameObject missionBtn;
    public GameObject DogFightBtn;
    public GameObject exitBtn;
    public AudioSource pointerHoverSound;
    public AudioSource pointerClickSound;


    public void OnMouseHover()
    {
        pointerHoverSound.Play();
    }
    public void OnStartBtn()
    {
        pointerClickSound.Play();
        gameObject.SetActive(false);
        tutorialBtn.SetActive(true);
        missionBtn.SetActive(true);
        DogFightBtn.SetActive(true);
        exitBtn.SetActive(true);
    }
    

    public void OnTutorialBtn()
    {
        pointerClickSound.Play();
        SceneManager.LoadScene("Tutorial");
    }
    public void OnMissionBtn()
    {
        pointerClickSound.Play();
        SceneManager.LoadScene("MissionMode");
    }
    public void OnDogFightBtn()
    {
        pointerClickSound.Play();
        SceneManager.LoadScene("DogFight_Scene");
    }
    public void OnExitBtn()
    {
        pointerClickSound.Play();
        print("Exit");
        Application.Quit();
    }
}
