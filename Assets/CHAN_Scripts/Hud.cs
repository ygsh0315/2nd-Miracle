﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hud : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private MouseFlightController mouseFlight = null;
    [SerializeField] private CHAN_Missile cm = null;

    [Header("HUD Elements")]
    [SerializeField] public RectTransform boresight = null;
    [SerializeField] private RectTransform mousePos = null;
    [SerializeField] private RectTransform seeker = null;
    [SerializeField] private RectTransform targetBox = null;
    [SerializeField] private RectTransform targetRedBox = null;

    bool isSeekerOn = false;
    public bool targetLock = false;
    float curTime = 0;

    private Camera playerCam = null;

    private void Awake()
    {
        if (mouseFlight == null)
            Debug.LogError(name + ": Hud - Mouse Flight Controller not assigned!");
        //mouse flightcontroller에서 사용중인 Camera를 불러온다.
        playerCam = mouseFlight.GetComponentInChildren<Camera>();

        if (playerCam == null)
            Debug.LogError(name + ": Hud - No camera found on assigned Mouse Flight Controller!");
        //seeker.gameObject.SetActive(false);
        targetBox.gameObject.SetActive(false);
        targetRedBox.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {

        if (mouseFlight == null || playerCam == null)
            return;

        UpdateGraphics(mouseFlight);




    }

    private void UpdateGraphics(MouseFlightController controller)
    {
        if (boresight != null)
        {
            boresight.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            boresight.gameObject.SetActive(boresight.position.z > 1f);

        }

        if (mousePos != null)
        {
            mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos);
            mousePos.gameObject.SetActive(mousePos.position.z > 1f);
        }
        // 쉬프트 계속누르면 씨커가 켜진다.
        if (Input.GetKey(KeyCode.R))
        {
            Seeking();
        }
        // 쉬프트를 떼면 씨커 꺼진다.
        else if (Input.GetKeyUp(KeyCode.R))
        {
            isSeekerOn = false;
            cm.readyToLanch = false;
            targetBox.gameObject.SetActive(false);
            targetRedBox.gameObject.SetActive(false);
            curTime = 0;


        }

        if (isSeekerOn)
        {

            seeker.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            seeker.gameObject.SetActive(true);
        }
        else
        {
            seeker.gameObject.SetActive(false);

        }

    }
    public void SetReferenceMouseFlight(MouseFlightController controller)
    {
        mouseFlight = controller;
    }


    // 씨커 기능 (적을 탐지했을 때 깜빡거리고 타깃이 락온되면 깜빡거림 멈춤)
    void Seeking()
    {


        curTime += Time.deltaTime;
        // 해당 루프는 타겟 안잡았을때
        if (!cm.isLocked)
        {
            targetBox.gameObject.SetActive(false);
            targetRedBox.gameObject.SetActive(false);
            if (curTime > 0.5f)
            {
                isSeekerOn = true;
                if (curTime > 1f)
                {
                    curTime = 0;
                    isSeekerOn = false;
                }
            }
        }
        else if (cm.isLocked)
        {
            //print("타깃 락");
            isSeekerOn = true;
            targetBox.gameObject.SetActive(true);
            targetBox.position = new Vector3(cm.targetPos.x, cm.targetPos.y);
            if (cm.finalLocked)
            {
                targetBox.gameObject.SetActive(false);
                targetRedBox.gameObject.SetActive(true);
                targetRedBox.position = new Vector3(cm.targetPos.x, cm.targetPos.y);
                cm.readyToLanch = true;
            }
        }

    }

}


