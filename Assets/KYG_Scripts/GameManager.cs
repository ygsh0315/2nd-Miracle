using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsPause;
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPause == false)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0;
                IsPause = true;
                return;
            }

            if (IsPause == true)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
                IsPause = false;
                return;
            }
        }
    }
}
