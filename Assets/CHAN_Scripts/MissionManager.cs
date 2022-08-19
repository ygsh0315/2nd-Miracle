using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    [SerializeField] Text playTime;

    // ���⼭ Ÿ�̸Ӹ� �����Ѵ�.
    //Ÿ�̸Ӵ� �ν����Ϳ��� ���� �����ϵ��� ����Ŵ�
    [Header ("TimeSet")]
    [SerializeField] float min;
    [SerializeField] float sec;
    float msec;
    
    
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
                M_Start();
                break;
            case State.mission1:
                M_1Start();
                break;
            case State.mission2:
                M_2Start();
                break;
            case State.mission3:
                M_3Start();
                break;
            case State.mission4:
                M_4Start();
                break;
            case State.End:
                M_5Start();
                break;
        }
    }





    private void M_Start()
    {
        // �̼��� ����
        // ���ѽð��ȿ� ���� ����϶�� ����
        // mission1���� ��ȯ
        
        state = State.mission1;
        


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
                StopAllCoroutines();
                print("timeOut ������");
            }
            else
            {
                min--;
                sec = 60;
            }
 
        }
        if (min == 0 && sec <= 30)
        {
            playTime.GetComponent<Text>().color = Color.red;
            StartCoroutine(Blink());
            //�ؽ�Ʈ�� �����Ÿ��� �����.   
        }
        if (missionCount == 10)
        {
            // �ð��ȿ� ��� �� ����ϸ� ���� �̼� ����
            state = State.mission2;
        }
    }
    private void M_2Start()
    {
        //�̰��� ��ǥ �ǹ��� Ÿ���ϴ� ����
        //���� �ǹ��� �ı��ߴٸ� �ش� �̼� Ŭ���� 
        state = State.mission3;
    }

    private void M_3Start()
    {
        //Ÿ�� �Ϸ��ϰ� ���õ� ������ ���������� ����
    }
    private void M_4Start()
    {
        //Ż�� �Ϸ��ϸ� ���� �ı� �̼� �ϴ�
        //���⸦ ��� �����ϸ� ������ �̼�����
    }
    private void M_5Start()
    {
        // �׸� �����ϰ� �����ϸ� ���� Ŭ����
    }

    void Timer()
    {
        sec = sec - Time.deltaTime;
        msec = msec - (Time.deltaTime);
        string Sec = string.Format("{0:00}", sec);
        string Msec = string.Format("{0:.00}", msec).Replace(".", "");
        playTime.text = $"{min}:{Sec}:{Msec}";
    }

    IEnumerator Blink()
    {
        float curTime=0;
        while (true)
        {
            curTime += Time.fixedDeltaTime;
            if(curTime>1f)
            {
                playTime.enabled = false;
            }
            if (curTime > 2f)
            {
                playTime.enabled = true;
                curTime = 0;
            }
            yield return null;
        }
    }
}
