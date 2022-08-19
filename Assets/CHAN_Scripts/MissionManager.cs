using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
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
        if (missionCount == 10)
        { 
            
            
        }
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.missionStart:
                MissionStart();
                break;
            case State.mission1:
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
        // Ÿ�̸� ���� 
    }
}
