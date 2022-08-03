using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적을 추적하는 미사일

public class LeadMissile : MonoBehaviour
{
    public static float LMspeed = 100;

    GameObject target;

    RaycastHit LcsHit;

    Vector3 targetDir;

    Vector3 dir;

    Vector3 impactPoint;

    float hitDistance;

    float distance;

    float ratio;



    
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerMove.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = 0.3f;
        if (!target)
        {
            dir = transform.forward;
        }
        else
        {
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out LcsHit))
            {
                distance = (target.transform.position - transform.position).magnitude;
                targetDir = (PlayerMove.instance.sc.transform.position - LcsHit.point).normalized;
                hitDistance = (target.transform.position - LcsHit.point).magnitude;             
                ratio = distance / hitDistance;
                impactPoint = transform.position + targetDir * LMspeed * ratio;
                dir = impactPoint - transform.position;
                //Debug.DrawLine(transform.position, target.transform.position, Color.red);
                //Debug.DrawLine(PlayerMove.instance.sc.transform.position, LcsHit.point, Color.green);
                //Debug.DrawLine(LcsHit.point, target.transform.position, Color.blue);
                //Debug.DrawLine(transform.position, impactPoint, Color.magenta);
            }
        }
        
        transform.forward = Vector3.Lerp(transform.forward, dir, 10*Time.deltaTime);
        transform.position += transform.forward.normalized * LMspeed * Time.deltaTime;
    }
}
