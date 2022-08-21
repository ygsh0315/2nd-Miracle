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

    [SerializeField] Text displayText = null;
    [SerializeField] Text displayText2 = null;

    // 출력 퍼센테이지
    float thrustPercent;
    // 브레이크 토크
    float brakesTorque;
    // 스크립트 가져오겠다는 뜻
    AircraftPhysics aircraftPhysics;
    public Rigidbody rb { get; set; }
    private bool rollOverride = false;
    private bool pitchOverride = false;
    [SerializeField]
    float AutopilotSensitivity = 8f;

    [Header("WEP Setting")]
    [SerializeField] float SetTime = 1;
    public CHAN_PlayerEffectManager Effect;
    Vector3 lastVelocity;
    public float acc;

    float curTime;
    public bool isWEP = false;

    [Header("G-LOC Setting")]
    [SerializeField] float LOCVel = 60;
    [SerializeField] public float PilotState{get;set;}
    float Damage;
    public bool canControl;
    public bool isSmoke;
    public bool isLeadSmoke;
    [SerializeField]  bool isFlap;

    [Header("Audio")]
    [SerializeField] CHAN_SoundManager sound = null;
    [Header("brake")]
    [SerializeField] Collider[] brake;
    int brakeSet;
    bool turn;
    float delayTime;
    bool isground;
    float waitTime;

    [SerializeField] GameObject missile;


    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        PilotState = 100;
        brakeSet = -1;

    }
    
    private void Update()
    {
        ControlVelocity();
        if (thrustPercent > 0&&!isWEP)
        {
            sound.moveState = CHAN_SoundManager.MoveState.Normal;
        }
        else if (thrustPercent<= 0)
        {
            sound.moveState = CHAN_SoundManager.MoveState.Idle;
        }
        acc = (rb.velocity.magnitude - lastVelocity.magnitude) / Time.fixedDeltaTime;
        sc.transform.position = transform.position + transform.forward.normalized * rb.velocity.magnitude * 0.01f;
        sc.GetComponent<SphereCollider>().radius = LeadMissile.LMspeed * 0.01f;
        //입력값을 받는다.
        if (canControl)
        {
            Pitch = Input.GetAxis("Vertical");
            Pitch = Mathf.Clamp(Pitch, -1, 0.2f);
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
                    if (!turn)
                    {
                        sound.boost.PlayOneShot(sound.audioClips[3], 1);
                        turn = true; 
                    }
                    delayTime += Time.deltaTime;
                    if (delayTime > 2)
                    {
                        sound.moveState = CHAN_SoundManager.MoveState.AfterBurner;
                        delayTime = 0;
                    }
                    if (isground)
                    {
                        thrustPercent = 4f;
                    }
                    else
                    {
                        thrustPercent = 2f;
                    }
                    
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
            turn = false;
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
            isFlap = Flap > 0 ? true : false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            brakeSet *= -1;
            if (brakeSet == -1)
            {
                if (rb.velocity.magnitude < 0.2f)
                {
                    isground = true;
                    brake[0].material.staticFriction = 100;
                    brake[1].material.staticFriction = 100;
                    brake[2].material.staticFriction = 100;
                    rb.drag = 100;
                    print("1111");
                }
                else 
                {
                    brake[0].material.dynamicFriction = 100;
                    brake[1].material.dynamicFriction = 100;
                    brake[2].material.dynamicFriction = 100;
                }
                
            }
            else if (brakeSet == 1)
            {
                
                brake[0].material.dynamicFriction = 0;
                brake[1].material.dynamicFriction = 0;
                brake[2].material.dynamicFriction = 0;
                rb.drag = 0;
            }
            //브레이크 on 되면 마찰 부여
            //브레이크 off 되면 마찰 해제
        }
        if (brakeSet == 1)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 2)
            {
                isground = false;
                waitTime = 0;
            }
        }
        // 출력값을 UI로 출력해 주는 부분 
        displayText.text = "속도\n";
        displayText.text += "고도\n";
        displayText.text += "스로틀\n";
        displayText.text += "조종사 의식\n";
        displayText.text += "기총\n";
        displayText.text += "미사일\n";
        displayText.text += "브레이크";
        displayText2.text =((int)rb.velocity.magnitude * 6).ToString("D3") + " km/h\n";
        displayText2.text +=((int)transform.position.y).ToString("D3") + " m\n";
        displayText2.text +=(int)(thrustPercent * 100) + "%\n";
        displayText2.text +=(int)(PilotState) + "%\n";
        displayText2.text +=(GetComponent<CHAN_Gun>().leftAmmo) + "\n";
        displayText2.text +=(GetComponent<CHAN_Missile>().leftMissile) + "\n";
        displayText2.text += brakeSet == -1 ? "ON" : "OFF";

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
        Damage = Pitch * rb.velocity.magnitude * 0.001f;
        //print(Damage);
        if ((Pitch<-0.3f)&&(rb.velocity.magnitude>LOCVel))
        {
            //파일럿의 체력이 점차 감소한다.
            if (isFlap)
            {
                PilotState += (Damage * 2.5f);
            }
            else
            {
                PilotState += Damage;
            }
            if (PilotState <= 0)
            {
                PilotState = 0;
            }

        }
        else
        {
            //해당안되면 체력 다시 증가
            Damage = 0;
            PilotState += 0.07f;
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

        lastVelocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            print("물 접촉");
            canControl = false;
            Effect.isDie=true;

    //이팩트 매니저에게 불타는 애니메이션 실행 호출
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            print("충돌");
            canControl = false;
            Effect.isDie = true;
        }
    }
    void ControlVelocity()
    {
        //비행기 속도가 일정속도 이상으로 증가 할 때 drag값을 증가시키는 함수
        //만약 속도값이 100이 넘어갈때 Drag값을 증가시킨다
        if (rb.velocity.magnitude > 125)
        {
            rb.drag = Mathf.Lerp(rb.drag, 1, Time.deltaTime);
            if (rb.drag >= 1)
            {
                rb.drag = 1;
            }

        }
        else if (rb.velocity.magnitude < 115&&!isground)
        {
            rb.drag = Mathf.Lerp(rb.drag, 0, Time.deltaTime);
            if (rb.drag <= 0)
            {
                rb.drag = 0;
            }
        }
    }

}
