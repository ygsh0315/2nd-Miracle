using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualButton : MonoBehaviour
{
    public GameObject Manual;
    GameManager gm;
    Image manualImage;
    // Start is called before the first frame update
    void Start()
    {
        manualImage = Manual.GetComponent<Image>();
        gm = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnManualBtn()
    {
        gm.PauseMenu.SetActive(false);
        Time.timeScale = 1;
        gm.IsPause = false;
        if(manualImage.enabled == false)
        {
            manualImage.enabled = true;
        }
        print(1);
    }
}
