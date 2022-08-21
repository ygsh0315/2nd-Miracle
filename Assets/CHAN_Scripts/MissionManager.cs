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

    // 여기서 타이머를 세팅한다.
    //타이머는 인스펙터에서 수정 가능하도록 만들거다
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

    //이것은 미션매니저
    // 미션1: 제한시간 안에 타겟지점에 도착하라
    // 미션2: 목표물을 파괴해라
    // 미션3: 적 항공기를 모두 파괴하라
    // 미션4: 항모에 착함해라
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
    // 트리거는 총2종류가 있다. 
    // 타미어 시작구간
    // 통과 구간
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
        // 미션을 시작
        // 제한시간안에 링을 통과하라고 지령
        // mission1으로 전환
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
        // 타이머 시작
        // second
        // 초는 60단위로 작동된다. 초가 0보다 낮아질 경우 60부터 시작하도록 만들어야한다.
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
                StartCoroutine(NarrationSay("적 출현", 10));
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
            //텍스트를 깜빡거리게 만든다.   
        }
        if (mission1Trigger.transform.childCount==0)
        {
            // 시간안에 모든 링 통과하면 다음 미션 시작
            // 링 갯수가 상시 변할 수 있으므로 계층구조에 존재하는 갯수를 파악하는 방법은?
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
        //이곳은 목표 건물을 타격하는 구간
        //만약 건물을 파괴했다면 해당 미션 클리어 
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
        //타격 완료하고 제시된 구간을 지나가도록 유도
        //지정된 구간에 탈출하라고 안내
        //이곳은 mission3Trigger를 만든다.
        
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
        //탈출 완료하면 적기 파괴 미션 하달
        //적기를 모두 격추하면 마지막 미션으로
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
        // 항모에 안전하게 착륙하면 게임 클리어
        // 플레이어가 지정된 trigger에 도착하고 속도가 0일 경우 게임이 끝나도록 유도
        //완전히 멈추고 지정된 시간동안 유지될 경우 클리어
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
    // 해당 함수는 항공기가 터지거나, 미션에 실패했을 경우 발동되는 함수
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
