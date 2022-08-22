using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool IsPause;
    public GameObject PauseMenu;
    public GameObject Manual;
    public GameObject text;
    public GameObject Radar;
    Image ManualImage;
    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;
        PauseMenu.SetActive(false);
        ManualImage = Manual.GetComponent<Image>();
        ManualImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F3))
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
}
