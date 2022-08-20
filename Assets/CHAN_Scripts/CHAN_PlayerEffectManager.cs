using System;
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
    [SerializeField] ParticleSystem burning1 = null;
    [SerializeField] ParticleSystem burning2 = null;


    [SerializeField] AirplaneController controller;
    [SerializeField] CHAN_SoundManager sound;
    [SerializeField]  Image image;
    [SerializeField] Text warningText;
    int n = 0;

    [SerializeField] float fadeSpeed;

    public bool isDie;
    float curTime;
    [SerializeField] float DelayToExplosion;



    void Start()
    {
        Color color = image.color;
        color.a = 0;
        smoke_R.SetActive(false);
        smoke_L.SetActive(false);
        leadSmoke_L.Stop();
        leadSmoke_R.Stop();
        warningText.enabled = false;
        //burning1.Play();
        //burning2.Play();





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

        if (isDie)
        {
            StartDie();
        }
    }



    public void WEP_ON()
    {
        if (!isDie)
        { 
            StartAfterBurner();
        }
        
    }

    public void WEP_OFF()
    {
        if (!isDie)
        { 
            EndAfterBurner();
        }
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
        // 가속도 -2~2 까지는 안전구간
        // 기 이상 이하부터 카메라 FOV 바뀌도록 유도
        
            
            if (controller.acc > 2)
            {
                Camera.main.fieldOfView += controller.acc * 0.02f;
            }
            if (controller.acc < -2)
            {
                Camera.main.fieldOfView += controller.acc * 0.02f;
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
        if (controller.isSmoke&&!isDie)
        {
            smoke_R.SetActive(true);
            smoke_L.SetActive(true);
        }
        else
        {
            smoke_R.SetActive(false);
            smoke_L.SetActive(false);
        }
        if (controller.isLeadSmoke&&!isDie)
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
            StartCoroutine(WarningUI("조종사 의식저하 9G", 1));
            PlayFadeIn();
            sound.Gloc();
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
            StopAllCoroutines();
            if (warningText.enabled)
            {
                warningText.enabled = false;
            }
            controller.canControl = true;
            PlayFadeOut();
        }
    }
    IEnumerator WarningUI(string say, float blinkTime)
    {
        // 위 함수는 경고문구
        // 깜빡거리면서 빨간글씨로 UI로 뜨게 한다.
        float curTime = 0;
        while (true)
        { 
            curTime += Time.fixedDeltaTime;
            if (curTime > blinkTime)
            {
                warningText.enabled = true;
                warningText.text = say;
            }
            if (curTime > blinkTime * 2)
            {
                warningText.enabled = false;
                curTime = 0;
            }
            yield return null;
        
        }
    }

    private void StartDie()
    {
        //여기서 불타는 이팩트 추가 할 예정 
        burning1.Play();
        burning2.Play();
        curTime += Time.deltaTime;
        if (curTime > DelayToExplosion)
        {
            //그리고 일정시간 지나면 폭발함
            //그리고 폭발 사운드
        }

        // 카메라는 그자리에서 대기
        // 미션 매니저에게 게임오버 반환
    }

}
