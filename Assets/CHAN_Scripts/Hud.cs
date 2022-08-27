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
    bool[] isBehind;
    public GameObject[] targetIcons;

    [Header("HUD Elements")]
    [SerializeField] public RectTransform boresight = null;
    [SerializeField] private RectTransform mousePos = null;
    [SerializeField] private RectTransform seeker = null;
    [SerializeField] private RectTransform targetBox = null;
    [SerializeField] private RectTransform targetRedBox = null;
    [SerializeField] private GameObject enemyPools = null;
    [SerializeField] private GameObject targetIcon = null;
    [SerializeField] private GameObject player = null;

    bool isSeekerOn = false;
    public bool targetLock = false;
    float curTime = 0;

    private Camera playerCam = null;

    private void Awake()
    {
        if (mouseFlight == null)
            Debug.LogError(name + ": Hud - Mouse Flight Controller not assigned!");
        //mouse flightcontroller에서 사용중인 Camera를 불러온다.
        playerCam = mouseFlight.GetComponentInChildren<Camera>();

        if (playerCam == null)
            Debug.LogError(name + ": Hud - No camera found on assigned Mouse Flight Controller!");
        //seeker.gameObject.SetActive(false);
        targetBox.gameObject.SetActive(false);
        targetRedBox.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


    }

    void Start()
    {
        targetIcons = new GameObject[enemyPools.transform.childCount];
        for (int i = 0; i < enemyPools.transform.childCount; i++)
        {
            targetIcons[i] = Instantiate(targetIcon,transform);
            targetIcons[i].SetActive(false);
        }
    }
    private void Update()
    {
        detect = Physics.OverlapSphere(player.transform.position, 5000, 1 << 7);
        dir = new Vector3[detect.Length];
        angle = new float[detect.Length];
        isBehind = new bool[detect.Length];
        for (int i = 0; i < detect.Length; i++)
        {
            dir[i] = (detect[i].transform.position - Camera.main.transform.position).normalized;
            angle[i] = Vector3.Angle(Camera.main.transform.forward, dir[i]);
            if (angle[i] > 90)
            {
                isBehind[i] = true;
            }
            else
            {
                isBehind[i] = false;
            }
            if (!isBehind[i]&&!detect[i].gameObject.GetComponent<Enemy>().isHit)
            {
                targetIcons[i].transform.position = Camera.main.WorldToScreenPoint(detect[i].transform.position);
                targetIcons[i].SetActive(true);

            }
            else if (isBehind[i])
            {
                targetIcons[i].SetActive(false);
            }
            if (detect[i].gameObject.GetComponent<Enemy>().isHit)
            {
                targetIcons[i].SetActive(false);
            }

            // 만약 중간에 타깃이 죽으면 색깔을 다른색으로 바꾼다. 
        }
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

}


