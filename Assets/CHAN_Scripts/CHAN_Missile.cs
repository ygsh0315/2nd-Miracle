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

    [SerializeField] List<float> meToTarget = new List<float>();

    // 일단은 호밍 미사일이 작동하는 것을 봐야하므로 타겟을 지정해두겠다.
    [SerializeField] public List<GameObject> target = null;
    [SerializeField] public List<GameObject> detected = new List<GameObject>();
    [SerializeField] Vector3[] dir = new Vector3[4];
    [SerializeField] float[] angle = new float[4];

    [SerializeField] Hud hud;
    Rigidbody Trb;
    Rigidbody rb;
    Vector2 seekerPos;
    public Vector3 targetPos;
    float curTime = 0;
    [SerializeField] float setTime = 1;
    int LaunchCount = 0;
    public bool isLocked { get; set; }
    public bool finalLocked { get; set; }
    // public List<GameObject> enemyPool = new List<GameObject>();

    public bool isLaunch;
    bool[] isBehind = new bool[4];

    public bool readyToLanch { get; set; }

    // 시작되자마자 미사일풀의 미사일을 비활성화 한다.
    void Awake()
    {
        for (int i = 1; i < missilePool.Count; i++)
        {
            missilePool[i - 1].SetActive(false);
        }


    }
    // 플레이어와 타겟 사이 거리를 측정할 리스트공간을 할당한다.
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 시작부분에서 게임 상 존재하는 모든 타겟의 정보가 list에 저장된다.
        for (int i = 0; i < target.Count; i++)
        {
            meToTarget.Add(0);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(Camera.main.WorldToScreenPoint(target[0].transform.position));
        }
        // 조준점의 좌표를 받는다.
        seekerPos = Camera.main.WorldToScreenPoint(MController.BoresightPos);
        if (target.Count != 0)
        {
            GetTargetPos();
        }
        else
        {
            return;
        }
        //플레이어와 타깃들 사이 거리를 계속해서 받는 함수
        if (Input.GetKey(KeyCode.R))
        {
            Seek1();
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
                print(LaunchCount);
            }
            LaunchCount++;
        }
    }

    public void RemoveTarget(GameObject t)
    {
        target.Remove(t);
    }

    // 발사버튼을 눌렀을 때 리스트에 담겨있는 미사일을 활성화 시키고 초기속도를 부여함
    void LaunchMissile(int Count)
    {
        fakeMissilePool[LaunchCount].SetActive(false);

        missilePool[Count].SetActive(true);
<<<<<<< HEAD:Assets/Assets/MouseFlight/Demo/Scripts/CHAN_Missile.cs
<<<<<<< Updated upstream
=======
        //미사일 스크립트에서 발사가 시작됐을 때 'Ondestroyed' 라는 Action 이 실행됨
        //그 Action 이 실행되면 이 스크립트의 'target' 리스트 항목이 지워짐
        //missilePool[Count].GetComponent<PlayerLeadMissile>().onDestroyed = RemoveTarget;
=======
>>>>>>> main:Assets/CHAN_Scripts/CHAN_Missile.cs
        missilePool[Count].GetComponent<PlayerLeadMissile>().onDestroyed = (t) =>
        {
            target.Remove(t);
        };
<<<<<<< HEAD:Assets/Assets/MouseFlight/Demo/Scripts/CHAN_Missile.cs
>>>>>>> Stashed changes
=======
>>>>>>> main:Assets/CHAN_Scripts/CHAN_Missile.cs
        missilePool[Count].transform.position = hardPoint[Count].transform.position;
        missilePool[Count].transform.rotation = hardPoint[Count].transform.rotation;
        missilePool[Count].GetComponent<Rigidbody>().velocity = rb.velocity;



    }

    //실시간으로 타겟의 위치를 탐지하는 함수
    void GetTargetPos()
    {
        for (int i = 0; i < target.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, target[i].transform.position);
            meToTarget[i] = distance;
            dir[i] = target[i].transform.position - transform.position;
            angle[i] = Vector3.Angle(transform.forward, dir[i]);
            if (angle[i] > 90)
            {
                isBehind[i] = true;
            }
            else
            {
                isBehind[i] = false;
            }


        }
    }

    void InitialSet()
    {
        detected.Clear();
        finalLocked = false;
        isLocked = false;
        curTime = 0;
        isLaunch = false;
    }
    void Seek1()
    {
        int index = -1;
        for (int j = 0; j < target.Count; j++)
        {
            // 만약 플레이어,타깃사이 거리가1000 m 이하이고 좌측쉬프트를 누르고 있을때
            if (meToTarget[j] < 1000 && !isBehind[j] && target[j].activeSelf)
            {
                if (index == -1) index = j;
                else if (meToTarget[index] > meToTarget[j])
                {
                    index = j;
                }
                //감지된 적 오브젝트를 리스트에 넣고 다시 검사
            }
        }
        if (index != -1)
        {
            detected.Add(target[index]);
        }
    }
    void Seek2()
    {
        targetPos = Camera.main.WorldToScreenPoint(detected[0].transform.position);
        // 스크린상 타겟좌표가 해당 범위안에 들었을 때 두번째 탐지 완료
        if ((seekerPos.x - 200 < targetPos.x && targetPos.x < seekerPos.x + 200 &&
            (seekerPos.y - 200 < targetPos.y && targetPos.y < seekerPos.y + 200)))
        {

            isLocked = true;
            curTime += Time.deltaTime;
            //print(curTime);
            //그 후 2초가 경과하면 발사준비 완료
            if (curTime > setTime)
            {
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

