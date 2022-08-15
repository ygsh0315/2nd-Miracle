using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    public GameObject sc;
    [SerializeField]
    // 날개 파츠의 스크립트 오브젝트 모음
    List<AeroSurface> controlSurfaces = null;
    //[SerializeField]
    // 바퀴모음
    // List<WheelCollider> wheels = null;
    [SerializeField]
    MouseFlightController controller = null;
    [SerializeField]
    // 플레이어 컨트롤러 민감도
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;


    // 입력값 범위
    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;
    [SerializeField]
    Text displayText = null;

    // 출력 퍼센테이지
    float thrustPercent;
    // 브레이크 토크
    float brakesTorque;
    // 스크립트 가져오겠다는 뜻
    AircraftPhysics aircraftPhysics;
    Rigidbody rb;
    private bool rollOverride = false;
    private bool pitchOverride = false;
    [SerializeField]
    float AutopilotSensitivity = 8f;

    [Header("WEP Setting")]
    [SerializeField] float SetTime = 1;
    public CHAN_PlayerEffectManager Effect;
    
    float curTime;
    bool isWEP = false;

    [Header("G-LOC Setting")]
    [SerializeField] float LOCVel = 60;
    [SerializeField] public float PilotState{get;set;}
    public bool canControl;
    public bool isSmoke;
    public bool isLeadSmoke;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        
        PilotState = 100;

    }

    private void Update()
    {
        sc.transform.position = transform.position + transform.forward.normalized * rb.velocity.magnitude * 0.01f;
        sc.GetComponent<SphereCollider>().radius = LeadMissile.LMspeed * 0.01f;

        //입력값을 받는다.
        if (canControl)
        {
            Pitch = Input.GetAxis("Vertical");
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");

            float autoYaw = 0f;
            float autoPitch = 0f;
            float autoRoll = 0f;

            if (Mathf.Abs(Roll) > .25f)
            {
                rollOverride = true;
            }
            else if (Mathf.Abs(Roll) < .25f)
            {
                rollOverride = false;
            }

            if (Mathf.Abs(Pitch) > .25f)
            {
                pitchOverride = true;

            }
            else if (Mathf.Abs(Pitch) < .25f)
            {
                pitchOverride = false;

            }
            if (controller != null)
                RunAutopilot(controller.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);

            Yaw = autoYaw;
            Pitch = (pitchOverride) ? Pitch : autoPitch;
            Roll = (rollOverride) ? Roll : autoRoll;
        }
        else
        {
            Pitch = Mathf.Lerp(Pitch, 0, Time.deltaTime);
            Roll = Mathf.Lerp(Roll, 0, Time.deltaTime);
            Yaw = Mathf.Lerp(Yaw, 0, Time.deltaTime);
        }
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (thrustPercent > 1)
            {
                curTime += Time.deltaTime;
                if (curTime > SetTime)
                {
                    isWEP = true;
                    thrustPercent = 1.2f;
                }
                else
                {
                    thrustPercent = 1;
                }
            }
            else
            {
                thrustPercent += 0.005f;
            }
        }
        else if(thrustPercent>1)
        {
            isWEP = false;
            thrustPercent = 1;
            curTime = 0;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (thrustPercent < 0)
            {
                thrustPercent = 0;
            }
            else
            {
                thrustPercent -= 0.005f;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            brakesTorque = brakesTorque > 0 ? 0 : 100f;
        }
        // 출력값을 UI로 출력해 주는 부분 
        displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
        displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
        displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
        displayText.text += "HP: " + (int)(PilotState) + "%\n";
        displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";

        if (transform.position.y > 100)
        {
            Transform frontWheel = transform.GetChild(1).GetChild(0).GetChild(1);
            Transform leftWheel = transform.GetChild(1).GetChild(0).GetChild(3);
            Transform rightWheel = transform.GetChild(1).GetChild(0).GetChild(2);
            frontWheel.localRotation = Quaternion.Lerp(frontWheel.localRotation, Quaternion.Euler(100, 0, 0), 1f * Time.deltaTime);
            leftWheel.localRotation = Quaternion.Lerp(leftWheel.localRotation, Quaternion.Euler(0, 0, 145), 1f * Time.deltaTime);
            rightWheel.localRotation = Quaternion.Lerp(rightWheel.localRotation, Quaternion.Euler(0, 0, -145), 1f * Time.deltaTime);
           
        }
        else
        {
            Transform frontWheel = transform.GetChild(1).GetChild(0).GetChild(1);
            Transform leftWheel = transform.GetChild(1).GetChild(0).GetChild(3);
            Transform rightWheel = transform.GetChild(1).GetChild(0).GetChild(2);
            frontWheel.localRotation = Quaternion.Lerp(frontWheel.localRotation, Quaternion.Euler(0, 0, 0), 1f * Time.deltaTime);
            leftWheel.localRotation = Quaternion.Lerp(leftWheel.localRotation, Quaternion.Euler(0, 0, 0), 1f * Time.deltaTime);
            rightWheel.localRotation = Quaternion.Lerp(rightWheel.localRotation, Quaternion.Euler(0, 0, 0), 1f * Time.deltaTime);
        }

        if (isWEP)
        {
            Effect.WEP_ON();
            
        }
        else
        {
            Effect.WEP_OFF();
           
        }

        //여기는 G-LOC 감지하는 코드
        if (Mathf.Abs(Pitch) > 0.95f && rb.velocity.magnitude > LOCVel)
        {
            //파일럿의 체력이 점차 감소한다.
            PilotState -= 0.05f;
            if (PilotState <= 0)
            {
                PilotState = 0;
            }

        }
        else
        {
            //해당안되면 체력 다시 증가
            PilotState += 0.05f;
            if (PilotState >= 100)
            {
                PilotState = 100;
            }
        }
        if (Mathf.Abs(Pitch) > 0.4f && rb.velocity.magnitude > LOCVel)
        {
            isSmoke = true;
            if (Mathf.Abs(Pitch) > 0.7f && rb.velocity.magnitude > LOCVel+10)
            {
                isLeadSmoke = true;
            }
            else 
            {
                isLeadSmoke = false;
            }
        }
        else 
        {
            isSmoke = false;
        }
    }

    private void FixedUpdate()
    {
        // 
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        // foreach (var wheel in wheels)
        // {
        //     wheel.brakeTorque = brakesTorque;
        //     // small torque to wake up wheel collider
        //     wheel.motorTorque = 0.01f;
        // }
    }
    // 입력값을 받아서 기동 방향 결정해주는 함수(상태머신이다.)
    // 내가 입력한 키가 yaw, pitch, roll인지 이 함수를 통해 판별해줌
    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }

    private void RunAutopilot(Vector3 flyTarget, out float Yaw, out float Pitch, out float Roll)
    {
        var localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * AutopilotSensitivity;
        var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

        Yaw = Mathf.Clamp(-localFlyTarget.x, -1f, 1f);
        Pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);

        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        var wingsLevelRoll = transform.right.y;
        var wingsLevelInfluence = Mathf.InverseLerp(0f, 10f, angleOffTarget);
        Roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }

    
}
