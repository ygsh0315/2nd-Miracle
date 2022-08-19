using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    [SerializeField] Text playTime;
    float min;
    float sec;
    string msec;
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
        mission3,
        mission4,
        End
    }
    public State state;
    // Ʈ���Ŵ� ��2������ �ִ�. 
    // Ÿ�̾� ���۱���
    // ��� ����
    public int missionCount;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        if (missionCount == 10)
        {
            
        }
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.missionStart:
                MissionStart();
                break;
            case State.mission1:
                Mission1Start();
                break;
            case State.mission2:
                break;
            case State.mission3:
                break;
            case State.mission4:
                break;
            case State.End:
                break;
        }
    }



    private void MissionStart()
    {
        // �̼��� ����
        // ���ѽð��ȿ� ���� ����϶�� ����
        // mission1���� ��ȯ
        print("111111111");
        state = State.mission1;
        min = 0;
        sec = 0;


    }
    private void Mission1Start()
    {
        // Ÿ�̸� ����
        // second
        sec = sec + Time.deltaTime;
        if (sec > 60)
        {
            min ++;
            sec = 0;
        }
        // ms
        msec = string.Format("{0:.00}", Time.time % 1).Replace(".", "");

        playTime.text = $"{min}:{(int)sec}:{msec}";

    }
}
