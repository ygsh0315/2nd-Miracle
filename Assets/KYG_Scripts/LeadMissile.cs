using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� �����ϴ� �̻���

public class LeadMissile : MonoBehaviour
{
    public static float LMspeed = 125;

    GameObject target;

    RaycastHit LcsHit;

    Vector3 targetDir;

    Vector3 dir;

    Vector3 impactPoint;

    float hitDistance;

    float distance;

    float ratio;

    float dot;

    GameObject LCS;

    bool isClose = false;

    public GameObject explosionFactory;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");

        LCS = GameObject.Find("LCS");
        //생성되자마자 텍스트 아이콘을 생성시킨다.
        // 해당아이콘의 위치는 현재 미사일의 위치를 스크린 좌표로 나타낸 위치이다.
        //없어지면 해당 아이콘을 지운다.




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
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out LcsHit)&&!isClose)
            {
                distance = (target.transform.position - transform.position).magnitude;
                targetDir = (LCS.transform.position - LcsHit.point).normalized;
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
        //dot = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized);
        if (distance < 10)
        {
            isClose = true;
            dir = transform.forward;
            Destroy(gameObject, 2f);
        }

        transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.position += transform.forward.normalized * LMspeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {

        
        if (collision.gameObject.name == "Player")
        {
            PlayerHP.Instance.HP -= 100;
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
        
    }
    //private void OnDestroy()
    //{
    //    GameObject explosion = Instantiate(explosionFactory);
    //    explosion.transform.position = transform.position;
    //}
}
