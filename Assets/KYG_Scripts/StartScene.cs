using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartScene : MonoBehaviour
{
    public GameObject StartBtn;
    public GameObject tutorialBtn;
    public GameObject missionBtn;
    public GameObject exitBtn;
    public GameObject LogoSpace;
    public Image Logo;
    public AudioSource pointerHoverSound;
    public AudioSource pointerClickSound;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
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
        exitBtn.SetActive(true);
        Logo.GetComponent<RectTransform>().anchoredPosition = new Vector3(Logo.GetComponent<RectTransform>().anchoredPosition.x, Logo.GetComponent<RectTransform>().anchoredPosition.y + 200);
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
    public void OnExitBtn()
    {
        pointerClickSound.Play();
        print("Exit");
        Application.Quit();
    }
    
}
