using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHAN_PlayerEffectManager : MonoBehaviour
{
    // 여기서 플레이어 관련된 효과를 종합적으로 다룬다.
    [SerializeField] float initialFOV = 60;
    [SerializeField] float targetFOV = 80;

    [SerializeField] float start = 0;
    [SerializeField] float end = 0;

    [SerializeField] GameObject smoke_R= null;
    [SerializeField] GameObject smoke_L= null;
    [SerializeField] ParticleSystem afterBurner_R = null;
    [SerializeField] ParticleSystem afterBurner_L = null;
    [SerializeField] ParticleSystem leadSmoke_R = null;
    [SerializeField] ParticleSystem leadSmoke_L = null;


    [SerializeField] AirplaneController controller;
    [SerializeField]  Image image;
    int n = 0;
    
   

    [SerializeField] float fadeSpeed;


    void Start()
    {
        Color color = image.color;
        color.a = 0;
        smoke_R.SetActive(false);
        smoke_L.SetActive(false);
        leadSmoke_L.Stop();
        leadSmoke_R.Stop();
        

    }

    // Update is called once per frame
    void Update()
    {
        // GLOC 함수
        GLOC();
        // 비행운 효과 함수
        SmokeEffect();
        // 가속도값에따른 카메라 시야각 설정
        CameraFOV();
    }

    public void WEP_ON()
    {
        //Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime);
        StartAfterBurner();
        
    }

    public void WEP_OFF()
    {
        //Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime);
        EndAfterBurner();
    }
    void PlayFadeIn()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = image.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(color.a, end, 0.01f);
        if (color.a > end)
        {
            color.a = end;
        }
        // 계산한 알파 값 다시 설정.  
        image.color = color;
        print(color.a);
        // Debug.Log(time);
    }
    void PlayFadeOut()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = image.color;
        // 알파 값 계산.  
        if (color.a < start)
        {
            color.a = start;
        }
        color.a = Mathf.Lerp(color.a, start, 0.001f);
        // 계산한 알파 값 다시 설정.  
        image.color = color;
        //print(color.a);
    }
    void StartAfterBurner()
    {
        ParticleSystem.MainModule burn_R = afterBurner_R.GetComponent<ParticleSystem>().main;
        ParticleSystem.MainModule burn_L = afterBurner_L.GetComponent<ParticleSystem>().main;
        
        Color R_Color = burn_R.startColor.color;
        Color L_Color = burn_L.startColor.color;
        R_Color.a = Mathf.Lerp(R_Color.a, 1, 0.1f*Time.deltaTime);
        L_Color.a = Mathf.Lerp(L_Color.a, 1, 0.1f*Time.deltaTime);
        burn_R.startColor = R_Color;
        burn_L.startColor = L_Color;

    }
    void EndAfterBurner()
    {
        ParticleSystem.MainModule burn_R = afterBurner_R.GetComponent<ParticleSystem>().main;
        ParticleSystem.MainModule burn_L = afterBurner_L.GetComponent<ParticleSystem>().main;

        Color R_Color = burn_R.startColor.color;
        Color L_Color = burn_L.startColor.color;
        R_Color.a = Mathf.Lerp(R_Color.a, 0,0.01f);
        L_Color.a = Mathf.Lerp(L_Color.a, 0,0.01f);
        if (R_Color.a < 0.01f)
        {
            R_Color.a = 0;
        }
        if (L_Color.a < 0.01f)
        {
            L_Color.a = 0;
        }
        burn_R.startColor = R_Color;
        burn_L.startColor = L_Color;

    }
    void CameraFOV()
    {
        if (controller.acc > 2)
        {
            Camera.main.fieldOfView += controller.acc * 0.01f;
            if (controller.acc > 4)
            {
                Camera.main.fieldOfView += controller.acc * 0.02f;
            }
        }
        if (controller.acc < 0.5f)
        {
            Camera.main.fieldOfView += controller.acc * 0.02f;
            if (controller.acc < -1)
            {
                Camera.main.fieldOfView += controller.acc * 0.02f;
            }
        }

        if (Camera.main.fieldOfView > targetFOV)
        {
            Camera.main.fieldOfView = targetFOV;
        }
        if (Camera.main.fieldOfView < initialFOV)
        {
            Camera.main.fieldOfView = initialFOV;
        }
    }
    void SmokeEffect()
    {
        if (controller.isSmoke)
        {
            smoke_R.SetActive(true);
            smoke_L.SetActive(true);
        }
        else
        {
            smoke_R.SetActive(false);
            smoke_L.SetActive(false);
        }
        if (controller.isLeadSmoke)
        {
            leadSmoke_L.Play();
            leadSmoke_R.Play();
        }
        else
        {
            leadSmoke_L.Stop();
            leadSmoke_R.Stop();
        }
    }
    void GLOC()
    {
        if (controller.PilotState <= 40)
        {
            PlayFadeIn();
            if (n - controller.PilotState <= 0)
            {

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
            PlayFadeOut();
        }
    }






}
