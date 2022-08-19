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
    //이것은 미션매니저
    // 미션1: 제한시간 안에 타겟지점에 도착하라
    // 미션2: 목표물을 파괴해라
    // 미션3: 적 항공기를 모두 파괴하라
    // 미션4: 항모에 착함해라
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
    // 트리거는 총2종류가 있다. 
    // 타미어 시작구간
    // 통과 구간
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
        // 미션을 시작
        // 제한시간안에 링을 통과하라고 지령
        // 타이머 시작 
    }
}
