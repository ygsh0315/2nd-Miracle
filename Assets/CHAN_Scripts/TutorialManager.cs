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

    //�ؽ�Ʈ ����ð�
    [SerializeField] float playTime;

    [SerializeField] AirplaneController controller;
    GameObject player;

    // ���⼭ Ÿ�̸Ӹ� �����Ѵ�.
    //Ÿ�̸Ӵ� �ν����Ϳ��� ���� �����ϵ��� ����Ŵ�
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

    //�̰��� �̼ǸŴ���
    // �̼�1: ���ѽð� �ȿ� Ÿ�������� �����϶�
    // �̼�2: ��ǥ���� �ı��ض�
    // �̼�3: �� �װ��⸦ ��� �ı��϶�
    // �̼�4: �׸� �����ض�
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
    // Ʈ���Ŵ� ��2������ �ִ�. 
    // Ÿ�̾� ���۱���
    // ��� ����
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
            //�ι�° é�ͷ� �Ѿ
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
            //�ι�° é�ͷ� �Ѿ
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
            //�ι�° é�ͷ� �Ѿ
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
            // �ͷ��� Ȱ��ȭ��Ų��.
            
            missile = Physics.OverlapSphere(player.transform.position, 10000, 1 << 8);
            if (missile.Length==0)
            {
                //4��° é��
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
            //���������� �Ѿ
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
    // �ش� �Լ��� �װ��Ⱑ �����ų�, �̼ǿ� �������� ��� �ߵ��Ǵ� �Լ�
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
