using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHAN_PlayerEffectManager : MonoBehaviour
{
    // 여기서 플레이어 관련된 효과를 종합적으로 다룬다.
    // 
    [SerializeField] float initialFOV = 60;
    [SerializeField] float targetFOV = 80;

    [SerializeField] float start = 0;
    [SerializeField] float end = 0;
    [SerializeField] float animTime=1;
    float curTime;
    [SerializeField] AirplaneController controller;
    [SerializeField]  Image image;
    int n = 0;
    [SerializeField] float fadeTime;


    void Start()
    {
        Color color = image.color;
        color.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.PilotState < 30)
        {
            if (n - controller.PilotState <= 0)
            {
                PlayFadeIn();
                print("fadeIn");
            }
            else if (n - controller.PilotState >= 0)
            {
                PlayFadeOut();
                print("fadeOut");
            }
            
            if (controller.PilotState < 10)
            { 
            controller.canControl = false;
            }

            n = (int)controller.PilotState;
        }
        else
        {
            controller.canControl = true;
            
        }

       



    }

    public void WEP_ON()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime);
        //이팩트 추가부분
        //카메라 shakiung 부분
    }

    public void WEP_OFF()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime);
    }
    void PlayFadeIn()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        curTime += Time.deltaTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = image.color;
        // 알파 값 계산.  
        color.a += 0.005f;
        if (color.a > end)
        {
            color.a = end;
        }
        // 계산한 알파 값 다시 설정.  
        image.color = color;
        // Debug.Log(time);
    }
    void PlayFadeOut()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        curTime += Time.deltaTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = image.color;
        // 알파 값 계산.  
        color.a -= 0.005f;
        if (color.a < start)
        {
            color.a = start;
        }
        // 계산한 알파 값 다시 설정.  
        image.color = color;
    }
    





}
