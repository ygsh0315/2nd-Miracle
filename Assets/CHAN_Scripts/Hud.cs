using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Hud : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private MouseFlightController mouseFlight = null;
    [SerializeField] private CHAN_Missile cm = null;
    [SerializeField] Collider[] detect;
    [SerializeField] Vector3[] dir;
    [SerializeField] float[] angle;
    [SerializeField] float[] distance;
    bool[] isBehind;
    public GameObject[] targetIcons;
    public GameObject[] targetDistanceTexts;
    public GameObject[] targetNameTexts;

    [Header("HUD Elements")]
    [SerializeField] public RectTransform boresight = null;
    [SerializeField] private RectTransform mousePos = null;
    [SerializeField] private RectTransform seeker = null;
    [SerializeField] private RectTransform targetBox = null;
    [SerializeField] private RectTransform targetRedBox = null;
    [SerializeField] private GameObject enemyPools = null;
    [SerializeField] private GameObject targetIcon = null;
    [SerializeField] private GameObject targetDistanceText = null;
    [SerializeField] private GameObject targetNameText = null;
    [SerializeField] private GameObject player = null;

    bool isSeekerOn = false;
    public bool targetLock = false;
    float curTime = 0;

    private Camera playerCam = null;


    private void Awake()
    {
        //mouse flightcontroller에서 사용중인 Camera를 불러온다.
        playerCam = mouseFlight.GetComponentInChildren<Camera>();
        //seeker.gameObject.SetActive(false);
        targetBox.gameObject.SetActive(false);
        targetRedBox.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start()
    {
        targetIcons = new GameObject[enemyPools.transform.childCount];
        targetDistanceTexts = new GameObject[enemyPools.transform.childCount];
        targetNameTexts=new GameObject[enemyPools.transform.childCount];


        for (int i = 0; i < enemyPools.transform.childCount; i++)
        {
            targetIcons[i] = Instantiate(targetIcon,transform);
            targetDistanceTexts[i] = Instantiate(targetDistanceText, transform);
            targetNameTexts[i] = Instantiate(targetNameText, transform);
            targetIcons[i].SetActive(false);
            targetIcons[i].SetActive(false);
            targetNameTexts[i].SetActive(false);
        }
    }
    void Update()
    {
        EnemyIndicator();
        if (mouseFlight == null || playerCam == null)
            return;

        UpdateGraphics(mouseFlight);

    }

    private void UpdateGraphics(MouseFlightController controller)
    {
        if (boresight != null)
        {
            boresight.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            boresight.gameObject.SetActive(boresight.position.z > 1f);

        }

        if (mousePos != null)
        {
            mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos)+new Vector3(0,-4,0);
            mousePos.gameObject.SetActive(mousePos.position.z > 1f);
        }
        // R 계속누르면 씨커가 켜진다.
        if (Input.GetKey(KeyCode.R))
        {
            Seeking();
        }
        // R 떼면 씨커 꺼진다.
        else if (Input.GetKeyUp(KeyCode.R))
        {
            isSeekerOn = false;
            cm.readyToLanch = false;
            targetBox.gameObject.SetActive(false);
            targetRedBox.gameObject.SetActive(false);
            curTime = 0;


        }

        if (isSeekerOn)
        {

            seeker.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            seeker.gameObject.SetActive(true);
        }
        else
        {
            seeker.gameObject.SetActive(false);
        }

    }
    public void SetReferenceMouseFlight(MouseFlightController controller)
    {
        mouseFlight = controller;
    }

    // 씨커 기능 (적을 탐지했을 때 깜빡거리고 타깃이 락온되면 깜빡거림 멈춤)
    void Seeking()
    {

        curTime += Time.deltaTime;
        // 해당 루프는 타겟 안잡았을때
        if (!cm.isLocked)
        {
            targetBox.gameObject.SetActive(false);
            targetRedBox.gameObject.SetActive(false);
            if (curTime > 0.5f)
            {
                isSeekerOn = true;
                if (curTime > 1f)
                {
                    curTime = 0;
                    isSeekerOn = false;
                }
            }
        }
        else if (cm.isLocked)
        {
            //print("타깃 락");
            isSeekerOn = true;
            targetBox.gameObject.SetActive(true);
            targetBox.position = new Vector2(cm.targetPos.x, cm.targetPos.y);
            if (cm.finalLocked)
            {
                targetBox.gameObject.SetActive(false);
                targetRedBox.gameObject.SetActive(true);
                targetRedBox.position = new Vector2(cm.targetPos.x, cm.targetPos.y);
                cm.readyToLanch = true;
            }
        }

    }
    void EnemyIndicator()
    {
        // 특정 범위에 들어왔을 때 타깃의 위치를 감지(여기서 5000은 감지거리)
        detect = Physics.OverlapSphere(player.transform.position, 5000, 1 << 7);
        dir = new Vector3[detect.Length];
        angle = new float[detect.Length];
        isBehind = new bool[detect.Length];
        distance = new float[detect.Length];
        for (int i = 0; i < (detect.Length); i++)
        {
            dir[i] = (detect[i].transform.position - Camera.main.transform.position).normalized;
            angle[i] = Vector3.Angle(Camera.main.transform.forward, dir[i]);
            distance[i] = Vector3.Distance(player.transform.position, detect[i].transform.position);
            targetDistanceTexts[i].GetComponent<Text>().text = (distance[i]*0.002f).ToString("0.00")+"  km";
            

            if (angle[i] > 90)
            {
                isBehind[i] = true;
            }
            else
            {
                isBehind[i] = false;
            }
            // 여기에서 타깃 건물과 적을 분리한다.
            if (detect[i].gameObject.CompareTag("targetBuilding"))
            {
                if (!isBehind[i] && !detect[i].gameObject.GetComponent<ENF>().isTargetHit)
                {
                    targetIcons[i].transform.position = Camera.main.WorldToScreenPoint(detect[i].transform.position)+new Vector3(0,8,0);
                    targetNameTexts[i].transform.position= Camera.main.WorldToScreenPoint(detect[i].transform.position) + new Vector3(0, 17, 0);
                    targetDistanceTexts[i].transform.position= Camera.main.WorldToScreenPoint(detect[i].transform.position) + new Vector3(0, -10, 0);
                    targetNameTexts[i].GetComponent<Text>().text = "Target";

                    targetNameTexts[i].SetActive(true);
                    targetDistanceTexts[i].SetActive(true);
                    targetIcons[i].SetActive(true);
                }
                else if (isBehind[i])
                {
                    targetIcons[i].SetActive(false);
                    targetDistanceTexts[i].SetActive(false);
                    targetNameTexts[i].SetActive(false);
                }
                if (detect[i].gameObject.GetComponent<ENF>().isTargetHit)
                {
                    targetIcons[i].SetActive(false);
                    targetDistanceTexts[i].SetActive(false);
                    targetNameTexts[i].SetActive(false);
                }
            }
            else if(detect[i].gameObject.CompareTag("target"))
            {
                if (!isBehind[i] && !detect[i].gameObject.GetComponent<Enemy>().isHit)
                {
                    targetIcons[i].transform.position = Camera.main.WorldToScreenPoint(detect[i].transform.position) + new Vector3(0, 8, 0);
                    targetNameTexts[i].transform.position = Camera.main.WorldToScreenPoint(detect[i].transform.position) + new Vector3(0, 17, 0);
                    targetDistanceTexts[i].transform.position = Camera.main.WorldToScreenPoint(detect[i].transform.position) + new Vector3(0, -10, 0);
                    targetNameTexts[i].GetComponent<Text>().text = "Enemy";

                    targetNameTexts[i].SetActive(true);
                    targetIcons[i].SetActive(true);
                    targetDistanceTexts[i].SetActive(true);
                }
                else if (isBehind[i])
                {
                    targetIcons[i].SetActive(false);
                    targetDistanceTexts[i].SetActive(false);
                    targetNameTexts[i].SetActive(false);
                }
                if (detect[i].gameObject.GetComponent<Enemy>().isHit)
                {
                    targetIcons[i].SetActive(false);
                    targetDistanceTexts[i].SetActive(false);
                    targetNameTexts[i].SetActive(false);
                }
            }
        }
    }


}


