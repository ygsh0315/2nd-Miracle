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

    [SerializeField] GameObject smoke_R= null;
    [SerializeField] GameObject smoke_L= null;


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

    }

    // Update is called once per frame
    void Update()
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
        

        // Image ������Ʈ�� ���� �� �о����.  
        Color color = image.color;
        // ���� �� ���.  
        color.a = Mathf.Lerp(color.a, end, 0.01f);
        if (color.a > end)
        {
            color.a = end;
        }
        // ����� ���� �� �ٽ� ����.  
        image.color = color;
        print(color.a);
        // Debug.Log(time);
    }
    void PlayFadeOut()
    {
        // ��� �ð� ���.  
        // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
        

        // Image ������Ʈ�� ���� �� �о����.  
        Color color = image.color;
        // ���� �� ���.  
        if (color.a < start)
        {
            color.a = start;
        }
        color.a = Mathf.Lerp(color.a, start, 0.001f);
        // ����� ���� �� �ٽ� ����.  
        image.color = color;
        //print(color.a);
    }
    





}
