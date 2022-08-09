using System;
using System.Collections.Generic;
using UnityEngine;

//���� �����ϴ� �̻���

public class PlayerLeadMissile : MonoBehaviour
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

    Transform ELCS;
    CHAN_Missile cm;
    public Action<GameObject> onDestroyed;

    bool isClose = false;

    public GameObject explosionFactory;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        cm = player.GetComponent<CHAN_Missile>();
        target = cm.detected[0];
        ELCS = target.transform.GetChild(0);
        onDestroyed(target);

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
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out LcsHit) && !isClose)
            {
                distance = (target.transform.position - transform.position).magnitude;
                targetDir = (ELCS.position - LcsHit.point).normalized;
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

        if (distance < 15 && Vector3.Angle(transform.forward, target.transform.position - transform.position) > 15)
        {
            isClose = true;
            dir = transform.forward;
        }

        transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.position += transform.forward.normalized * LMspeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = collision.transform.position;
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
