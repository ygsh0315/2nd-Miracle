using UnityEngine;

public class mouseFlightController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]Transform aircraft = null;
    [SerializeField]Transform mouseAim = null;
    [SerializeField]Transform cameraRig = null;
    [SerializeField]Transform cam= null;

    [Header("Options")]
    [SerializeField]bool isFixed = true;
    [SerializeField]float camSmoothSpeed=5f;
    [SerializeField]float mouseSensitivity=3f;
    [SerializeField]float aimDistance=500f;
    Vector3 frozenDirection = Vector3.forward;
    bool isMouseFrozen = false;
  

    public Vector3 BoresightPos
    {
        get 
        {
            return aircraft == null
                 ? transform.forward*aimDistance
                 : (aircraft.transform.forward*aimDistance)+ aircraft.transform.position;
        }
    }

    public Vector3 MouseAimPos
    {
        get
        {
            if(mouseAim!=null)
            {
                return isMouseFrozen
                     ? GetFrozenMouseAimPos()
                     : mouseAim.position+(mouseAim.forward*aimDistance);
            }
            else
            {
                return transform.forward*aimDistance;
            }
        }
    }
    private void Awake()
        {
            transform.parent = null;
        }

 
    void Update()
    {
        if(isFixed ==false)
        {
            UpdateCameraPos();
        }
        RotateRig();
        print(frozenDirection);

        //마우스 회전 함수 호출
    }

    private void FixedUpdate() 
    {
        if(isFixed ==true)
        {
            UpdateCameraPos();
        }
    }
    Vector3 GetFrozenMouseAimPos()
    {
        if(mouseAim!=null)
        {
            return mouseAim.position +(frozenDirection*aimDistance);
        }
        else
        {
            return transform.forward*aimDistance;
        }
    }
    void UpdateCameraPos()
    {
        transform.position=aircraft.position;
    }

    void RotateRig()
    {
        if(mouseAim==null||cam==null||cameraRig==null)
            return;
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            //마우스에임 고정
            isMouseFrozen=true;
            
            //현재 기수방향 저장
            frozenDirection=mouseAim.forward;
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            //마우스에임 고정헤제
            isMouseFrozen=false;
            
            //저장한 방향을 현재 기수 방향으로 지정
            mouseAim.forward=frozenDirection;
        }

        //마우스 입력값을 받는다.
        float mx=Input.GetAxis("Mouse X")*mouseSensitivity;
        float my=-Input.GetAxis("Mouse Y")*mouseSensitivity;

        mouseAim.Rotate(cam.right,my,Space.World);
        mouseAim.Rotate(cam.up,mx,Space.World);

        Vector3 upVector=(Mathf.Abs(mouseAim.forward.y))>0.9f?cameraRig.up:Vector3.up;

        cameraRig.rotation=Damp(cameraRig.rotation,Quaternion.LookRotation(mouseAim.forward,upVector),camSmoothSpeed,Time.deltaTime);
    }
    Quaternion Damp(Quaternion a,Quaternion b, float lambda,float dt)
    {
        return Quaternion.Slerp(a,b,1-Mathf.Exp(-lambda*dt));
    }

}
