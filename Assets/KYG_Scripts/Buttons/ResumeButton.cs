using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonResume()
    {
        gm.PauseMenu.SetActive(false);
        Time.timeScale = 1;
        gm.IsPause = false;
    }
}
