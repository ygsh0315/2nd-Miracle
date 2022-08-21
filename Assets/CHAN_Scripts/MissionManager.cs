using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    [SerializeField] Text CountDown;
    [SerializeField] Text Narration;
    [SerializeField] GameObject mission1Trigger;
    [SerializeField] GameObject mission3Trigger;
    [SerializeField] GameObject TargetBuilding;
    [SerializeField] GameObject Enemys;

    [Header("narration Set")]
    [SerializeField] string M_1Text = "";
    [SerializeField] string M_2Text = "";
    //[SerializeField] string M_3Text = "";
    [SerializeField] string M_4Text = "";
    [SerializeField] string M_5Text = "";
    [SerializeField] float playTime;

    [SerializeField] AirplaneController controller;
    GameObject player;

    // ���⼭ Ÿ�̸Ӹ� �����Ѵ�.
    //Ÿ�̸Ӵ� �ν����Ϳ��� ���� �����ϵ��� ����Ŵ�
    [Header ("TimeSet")]
    [SerializeField] float min;
    [SerializeField] float sec;
    float msec;
    float waitTime;
    public bool isLand;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //�̰��� �̼ǸŴ���
    // �̼�1: ���ѽð� �ȿ� Ÿ�������� �����϶�
    // �̼�2: ��ǥ���� �ı��ض�
    // �̼�3: �� �װ��⸦ ��� �ı��϶�
    // �̼�4: �׸� �����ض�
    public enum State
    { 
        Idle,
        missionStart,
        mission1,
        mission2,
        //mission3,
        mission4,
        mission5,
        End,
        missionFail
    }
    public State state;
    // Ʈ���Ŵ� ��2������ �ִ�. 
    // Ÿ�̾� ���۱���
    // ��� ����
    public int missionCount;
    void Start()
    {
        player = GameObject.Find("PlayerObject");
        state = State.Idle;
        CountDown.enabled = false;
        waitTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

        if (!player.activeSelf)
        {
            state = State.missionFail;
        }
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.missionStart:
                M_Start();
                break;
            case State.mission1:
                M_1Start();
                break;
            case State.mission2:
                M_2Start();
                break;
            //case State.mission3:
            //    M_3Start();
            //    break;
            case State.mission4:
                M_4Start();
                break;
            case State.mission5:
                M_5Start();
                break;
            case State.End:
                Ending();
                break;
            case State.missionFail:
                MissionFail();
                print("fail");
                break;
        }
    }

    

    private void M_Start()
    {
        // �̼��� ����
        // ���ѽð��ȿ� ���� ����϶�� ����
        // mission1���� ��ȯ
        StartCoroutine(NarrationSay(M_1Text, playTime));
        waitTime += Time.deltaTime;
        if(waitTime>2)
        {
            waitTime = 0;
            CountDown.enabled = true;
            state = State.mission1;
        }
        

    }
    private void M_1Start()
    {
        // Ÿ�̸� ����
        // second
        // �ʴ� 60������ �۵��ȴ�. �ʰ� 0���� ������ ��� 60���� �����ϵ��� �������Ѵ�.
        Timer();
        if (msec <= 0)
        {
            msec = 0.99f;
        }
        if (sec <=0)
        {
            if (min == 0)
            {
                sec = 0;
                msec = 0;
                if (!CountDown.enabled)
                { 
                    CountDown.enabled = true;
                }
                StartCoroutine(NarrationSay("�� ����", 10));
            }
            else
            {
                min--;
                sec = 60;
            }
 
        }
        if (min == 0 && sec <= 30&&sec>=0)
        {
            CountDown.GetComponent<Text>().color = Color.red;
            StartCoroutine(Blink());
            //�ؽ�Ʈ�� �����Ÿ��� �����.   
        }
        if (mission1Trigger.transform.childCount==0)
        {
            // �ð��ȿ� ��� �� ����ϸ� ���� �̼� ����
            // �� ������ ��� ���� �� �����Ƿ� ���������� �����ϴ� ������ �ľ��ϴ� �����?
            StartCoroutine(NarrationSay(M_2Text, playTime));
            waitTime += Time.deltaTime;
            if (waitTime > 2)
            {
                waitTime = 0;
                CountDown.enabled = false;
                state = State.mission2;
            }
            
        }
    }
    private void M_2Start()
    {
        //�̰��� ��ǥ �ǹ��� Ÿ���ϴ� ����
        //���� �ǹ��� �ı��ߴٸ� �ش� �̼� Ŭ���� 
        if (!TargetBuilding)
        {
            StartCoroutine(NarrationSay(M_4Text, playTime));
            waitTime += Time.deltaTime;
            if (waitTime > 2)
            {
                waitTime = 0;
                CountDown.enabled = false;
                state = State.mission4;
            }
            
        }
    }

    private void M_3Start()
    {
        //Ÿ�� �Ϸ��ϰ� ���õ� ������ ���������� ����
        //������ ������ Ż���϶�� �ȳ�
        //�̰��� mission3Trigger�� �����.
        
        if (mission3Trigger.transform.childCount == 0)
        {
            StartCoroutine(NarrationSay(M_5Text, playTime));
            waitTime += Time.deltaTime;
            if (waitTime > 2)
            {
                waitTime = 0;
                CountDown.enabled = false;
                state = State.mission5;
            }
            
        }
    }
    private void M_4Start()
    {
        //Ż�� �Ϸ��ϸ� ���� �ı� �̼� �ϴ�
        //���⸦ ��� �����ϸ� ������ �̼�����
        if (Enemys.transform.childCount == 0)
        {
            StartCoroutine(NarrationSay(M_5Text, playTime));
            waitTime += Time.deltaTime;
            if (waitTime > 2)
            {
                waitTime = 0;
                CountDown.enabled = false;
                state = State.mission5;
            }
            
        }

    }
    
    private void M_5Start()
    {
        // �׸� �����ϰ� �����ϸ� ���� Ŭ����
        // �÷��̾ ������ trigger�� �����ϰ� �ӵ��� 0�� ��� ������ �������� ����
        //������ ���߰� ������ �ð����� ������ ��� Ŭ����
        if (isLand==true&&controller.rb.velocity.magnitude<=0.05f)
        {
            waitTime += Time.deltaTime;
            if(waitTime>2)
            {
                state = State.End;
            }
            
        }
    }

    private void Ending()
    {
        print("Mission Complete");
    }
    // �ش� �Լ��� �װ��Ⱑ �����ų�, �̼ǿ� �������� ��� �ߵ��Ǵ� �Լ�
    private void MissionFail()
    {
        print("missionFail");
    }

    void Timer()
    {
        if (!(min == 0 && sec <= 0))
        {
            sec = sec - Time.deltaTime;
            msec = msec - (Time.deltaTime);
        }
        string Sec = string.Format("{0:00}", sec);
        string Msec = string.Format("{0:.00}", msec).Replace(".", "");
        CountDown.text = $"{min}:{Sec}:{Msec}";
    }

    IEnumerator Blink()
    {
        float curTime=0;
        while (true)
        {
            curTime += Time.fixedDeltaTime;
            if(curTime>1f)
            {
                CountDown.enabled = false;
            }
            if (curTime > 2f)
            {
                CountDown.enabled = true;
                curTime = 0;
            }
            yield return null;
        }
    }
    IEnumerator NarrationSay(string say, float t)
    {
        float delay = 0;
        Narration.enabled = true;
        Narration.text = say;
        while (true)
        {
            delay += Time.fixedDeltaTime;
            print(delay);
            if (delay > t)
            {
                Narration.enabled = false;
                yield break;
            }
            yield return null;
        }
    }
    
}
