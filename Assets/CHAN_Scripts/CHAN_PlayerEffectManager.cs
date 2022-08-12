using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHAN_PlayerEffectManager : MonoBehaviour
{
    // ���⼭ �÷��̾� ���õ� ȿ���� ���������� �ٷ��.
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
        //����Ʈ �߰��κ�
        //ī�޶� shakiung �κ�
    }

    public void WEP_OFF()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime);
    }
    void PlayFadeIn()
    {
        // ��� �ð� ���.  
        // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
        curTime += Time.deltaTime;

        // Image ������Ʈ�� ���� �� �о����.  
        Color color = image.color;
        // ���� �� ���.  
        color.a += 0.005f;
        if (color.a > end)
        {
            color.a = end;
        }
        // ����� ���� �� �ٽ� ����.  
        image.color = color;
        // Debug.Log(time);
    }
    void PlayFadeOut()
    {
        // ��� �ð� ���.  
        // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
        curTime += Time.deltaTime;

        // Image ������Ʈ�� ���� �� �о����.  
        Color color = image.color;
        // ���� �� ���.  
        color.a -= 0.005f;
        if (color.a < start)
        {
            color.a = start;
        }
        // ����� ���� �� �ٽ� ����.  
        image.color = color;
    }
    





}
