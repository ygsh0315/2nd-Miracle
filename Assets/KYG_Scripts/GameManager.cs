using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool IsPause;
    public Image FadeOutImage;
    public GameObject PauseMenu;
    public GameObject Manual;
    public GameObject text;
    public GameObject Radar;
    Image ManualImage;
    bool isFade = true;
    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;
        PauseMenu.SetActive(false);
        ManualImage = Manual.GetComponent<Image>();
        ManualImage.enabled = false;
        text.SetActive(false);
        Radar.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            StartCoroutine(FadeOut());
            isFade = false;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if(ManualImage.enabled == false)
            {
                ManualImage.enabled = true;
                text.SetActive(false);
                Radar.SetActive(false);
            }
            else
            {
                ManualImage.enabled = false;
                text.SetActive(true);
                Radar.SetActive(true);
            }
        }
       
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("MissionMode");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPause == false)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0;
                IsPause = true;
                Cursor.visible = true;
                return;
            }

            if (IsPause == true)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
                IsPause = false;
                Cursor.visible = false;
                return;
            }
        }
    }
    public IEnumerator FadeOut()
    {
        for(float i = 1f; i>=0; i-= Time.deltaTime/10)
        {
            FadeOutImage.color = new Color(0, 0, 0, i);
            if(i <0.1f)
            {
                text.SetActive(true);
                Radar.SetActive(true);
            }
            yield return null;
        }
    }
}
