using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSet : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera MainCam;
    public Camera SubCam;

    public void ShowMainView()
    {
        MainCam.enabled=true;
        SubCam.enabled=false;
    }

    public void ShowMissileView()
    {
        MainCam.enabled=false;
        SubCam.enabled=true;
    }
    void Start()
    {
        ShowMainView();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ShowMissileView();
        }
        if(Input.GetKeyUp(KeyCode.I))
        {
            ShowMainView();
        }
    }
}
