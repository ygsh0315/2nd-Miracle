using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CHAN_Missile : MonoBehaviour
{

    [SerializeField] Transform[] hardPoint;
    [SerializeField] MouseFlightController MController;
    [SerializeField] public List<GameObject> missilePool = new List<GameObject>();
    [SerializeField] List<GameObject> fakeMissilePool = null;
    // 일단은 호밍 미사일이 작동하는 것을 봐야하므로 타겟을 지정해두겠다.
    [SerializeField] Vector3[] dir;
    [SerializeField] float[] angle;
    [SerializeField]  Collider[] detect;
    [SerializeField] public List<GameObject> detected = new List<GameObject>();

    [SerializeField] Hud hud;
    Rigidbody rb;
    Vector2 seekerPos;
    public Vector3 targetPos;
    float curTime = 0;
    [SerializeField] float setTime = 1;
    int LaunchCount = 0;
    public bool isLocked { get; set; }
    public bool finalLocked { get; set; }
    public bool isLaunch;
    bool[] isBehind ;
    public bool readyToLanch { get; set; }
    public float leftMissile;

    // 시작되자마자 미사일풀의 미사일을 비활성화 한다.
    void Awake()
    {
        for (int i = 0; i < missilePool.Count; i++)
        {
            missilePool[i].SetActive(false);
        }


    }
    // 플레이어와 타겟 사이 거리를 측정할 리스트공간을 할당한다.
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 시작부분에서 게임 상 존재하는 모든 타겟의 정보가 list에 저장된다.
        leftMissile = missilePool.Count;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Seek1();
            CHAN_SoundManager.instance.attackState = CHAN_SoundManager.AttackState.seeking;
            if (detected.Count > 0)
            {
                Seek2();
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            InitialSet();
        }

        if (Input.GetKeyDown(KeyCode.Space) && readyToLanch)
        {
            //미사일 런치

            if (LaunchCount > 3)
            {

            }
            else
            {
                isLaunch = true;
                LaunchMissile(LaunchCount);
                leftMissile--;
            }
            LaunchCount++;
        }
    }

    // 발사버튼을 눌렀을 때 리스트에 담겨있는 미사일을 활성화 시키고 초기속도를 부여함
    void LaunchMissile(int Count)
    {
        fakeMissilePool[LaunchCount].SetActive(false);

        missilePool[Count].SetActive(true);
        missilePool[Count].transform.position = hardPoint[Count].transform.position;
        missilePool[Count].transform.rotation = hardPoint[Count].transform.rotation;
        missilePool[Count].GetComponent<Rigidbody>().velocity = rb.velocity;
    }
    void InitialSet()
    {
        CHAN_SoundManager.instance.attackState = CHAN_SoundManager.AttackState.Idle;
        detected.Clear();
        finalLocked = false;
        isLocked = false;
        curTime = 0;
        isLaunch = false;
    }
    void Seek1()
    {
        detect = Physics.OverlapSphere(transform.position, 1000,1<<7);
        dir = new Vector3[detect.Length];
        angle = new float[detect.Length];
        isBehind=new bool[detect.Length];
        for (int i=0;i<detect.Length;i++)
        {
            dir[i] = detect[i].transform.position - transform.position;
            angle[i] = Vector3.Angle(transform.forward, dir[i]);
            if (angle[i] > 90)
            {
                isBehind[i] = true;
            }
            else
            {
                isBehind[i] = false;
            }
            if (!isBehind[i])
            { 
                detected.Add(detect[i].transform.gameObject);
            }
        }
    }
    void Seek2()
    { 

        targetPos = Camera.main.WorldToScreenPoint(detected[0].transform.position);
        seekerPos = hud.boresight.position;
        // 스크린상 타겟좌표가 해당 범위안에 들었을 때 두번째 탐지 완료
        if ((seekerPos.x - 200 < targetPos.x && targetPos.x < seekerPos.x + 200) &&
            (seekerPos.y - 200 < targetPos.y && targetPos.y < seekerPos.y + 200))
        {
            isLocked = true;
            curTime += Time.deltaTime;
            //print(curTime);
            //그 후 2초가 경과하면 발사준비 완료
            if (curTime > setTime)
            {
                CHAN_SoundManager.instance.attackState = CHAN_SoundManager.AttackState.Lock;
                finalLocked = true;
            }
        }
        // 만약 중간에 감지 범위가 벗어나거나 락온을 취소할 경우, 초기화
        else
        {
            InitialSet();
            //print("2차" + curTime);
        }
    }
}

