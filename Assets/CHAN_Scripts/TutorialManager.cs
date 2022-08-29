using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TutorialManager : MonoBehaviour

{
    public static TutorialManager instance;
    [SerializeField] Text Narration;
    [SerializeField] GameObject mission1Trigger;

    [SerializeField] Collider[] missile;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject enemyGroup;


    [Header("Section Start text Setting")]
    [SerializeField] string[] CStart_Text = null;
    int cStart_Count = 0;
    [Header("Section1 text Setting")]
    [SerializeField] string[] C1_Text = null;
    int c1_Count=0;
    [Header("Section2 text Setting")]
    [SerializeField] string[] C2_Text = null;
    int c2_Count;
    [Header("Section3 text Setting")]
    [SerializeField] string[] C3_Text = null;
    int c3_Count;
    [Header("Section4 text Setting")]
    [SerializeField] string[] C4_Text = null;
    int c4_Count;
    [Header("Section5 text Setting")]
    [SerializeField] string[] C5_Text = null;
    int c5_Count;
    [Header("End text Setting")]
    [SerializeField] string[] E_Text = null;


    [SerializeField] string M_4Text = "";
    [SerializeField] string M_5Text = "";

    //텍스트 재생시간
    [SerializeField] float playTime;

    [SerializeField] AirplaneController controller;
    GameObject player;

    // 여기서 타이머를 세팅한다.
    //타이머는 인스펙터에서 수정 가능하도록 만들거다
    [Header("TimeSet")]
    [SerializeField] float min;
    [SerializeField] float sec;
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
        ChepterStart,
        Chepter1,
        Chepter2,
        Chepter3,
        Chepter4,
        Chepter5,
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
        state = State.ChepterStart;
        //CountDown.enabled = false;
        waitTime = 0;
        turret.SetActive(false);
        enemyGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.ChepterStart:
                C_Start();
                break;
            case State.Chepter1:
                C1_Start();
                break;
            case State.Chepter2:
                C2_Start();
                break;
            case State.Chepter3:
                C3_Start();
                break;
            case State.Chepter4:
                C4_Start();
                break;
            case State.Chepter5:
                C5_Start();
                break;
            case State.End:
                Ending();
                break;
            case State.missionFail:
                StopAllCoroutines();
                StartCoroutine(delayScene());
                state = State.Idle;
                break;
        }
    }



    private void C_Start()
    {
        if (cStart_Count >= CStart_Text.Length)
        {
            //두번째 챕터로 넘어감
            Narration.enabled = false;
            state = State.Chepter1;
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(CStart_Text[cStart_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                cStart_Count++;
                StopAllCoroutines();
            }
        }
    }
    private void C1_Start()
    {
        if (c1_Count >= C1_Text.Length)
        {
            //두번째 챕터로 넘어감
            Narration.enabled = false;
            StartCoroutine(delay(5, State.Chepter2));
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(C1_Text[c1_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c1_Count++;
                StopAllCoroutines();
            }
        }
    }
    private void C2_Start()
    {
        if (c2_Count >= C2_Text.Length)
        {
            //두번째 챕터로 넘어감
            Narration.enabled = false;
            if (mission1Trigger.transform.childCount == 0)
            {
                state = State.Chepter3;
            }
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(C2_Text[c2_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c2_Count++;
                StopAllCoroutines();
            }
        }
    }
    private void C3_Start()
    {
        turret.SetActive(true);
        if (c3_Count >= C3_Text.Length)
        {
            
            Narration.enabled = false;
            // 터렛을 활성화시킨다.
            
            missile = Physics.OverlapSphere(player.transform.position, 10000, 1 << 8);
            if (missile.Length==0)
            {
                //4번째 챕터
                state = State.Chepter4;
            }
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(C3_Text[c3_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c3_Count++;
                StopAllCoroutines();
            }
        }
    }
    private void C4_Start()
    {
        if (c4_Count >= C4_Text.Length)
        {
            Narration.enabled = false;
            enemyGroup.SetActive(true);
            if (enemyGroup.transform.childCount == 0)
            {
                state = State.Chepter5;
            }
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(C4_Text[c4_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c4_Count++;
                StopAllCoroutines();
            }
        }
    }
    private void C5_Start()
    {
        if (c5_Count >= C5_Text.Length)
        {
            Narration.enabled = false;
            if (isLand&&controller.rb.velocity.magnitude<= 0.05f)
            {
                c5_Count = 0;
                state = State.End;
            }
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(C5_Text[c5_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c5_Count++;
                StopAllCoroutines();
            }
        }
    }

    private void Ending()
    {
        if (c5_Count >= E_Text.Length)
        {
            Narration.enabled = false;
            //다음씬으로 넘어감
        }
        else
        {
            waitTime += Time.fixedDeltaTime;
            StartCoroutine(NarrationSay(E_Text[c5_Count], playTime));
            if (waitTime > playTime)
            {
                waitTime = 0;
                c5_Count++;
                StopAllCoroutines();
            }
        }
    }
    // 해당 함수는 항공기가 터지거나, 미션에 실패했을 경우 발동되는 함수
    private void MissionFail()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Fail_Scene");
    }


    IEnumerator NarrationSay(string say, float t)
    {
        float delay = 0;
        Narration.enabled = true;
        Narration.text = say;
        while (true)
        {
            delay += Time.fixedDeltaTime;
            if (delay > t)
            {
                Narration.enabled = false;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator delay(float delayTime,State chepter)
    {
        yield return new WaitForSeconds(delayTime);
        state =chepter;
    }
    IEnumerator delayScene()
    {
        yield return new WaitForSeconds(3);
        MissionFail();
    }

}
