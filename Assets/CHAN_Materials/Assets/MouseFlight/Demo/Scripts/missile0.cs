using System;
using System.Collections.Generic;
using UnityEngine;


public class missile0 : MonoBehaviour
{
    Transform target;


    [SerializeField] public GameObject player;


    Rigidbody rb;
    CHAN_Missile cm;
    public float accelleration = 2f;
    float acc;
    private Vector3 targetOld;
    private Quaternion guideRotation;
    public float missileSpeed = 10;
    float accSpeed;
    float curTime;

    public Action<GameObject> onDestroyed;

    void Start()
    {

        cm = player.GetComponent<CHAN_Missile>();
        if (cm.detected.Count != 0)
        {
            target = cm.detected[0].transform;
        }
        else
        {
            return;
        }
        rb = GetComponent<Rigidbody>();

        //Physics.gravity *= 1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        curTime += Time.deltaTime;
        rb.AddForce(Vector3.down, ForceMode.Force);
        if (curTime > 0.7f)
        {
            missileLaunch(target);
        }
    }


    void missileLaunch(Transform target)
    {
        float dist = Vector3.Distance(target.position, transform.position);
        Vector3 relativePos = target.position - transform.position;
        Vector3 targetVel = target.position - targetOld;
        //print(targetVel);
        targetVel /= Time.fixedDeltaTime;

        float privateVel = targetVel.magnitude;
        //print(privateVel);
        float timeToImpact = dist / privateVel;
        Vector3 leadPos = target.position + targetVel * timeToImpact;
        Vector3 leadVec = leadPos - transform.position;

        relativePos = Vector3.RotateTowards(relativePos.normalized, leadVec.normalized, 45 * Mathf.Deg2Rad * 0.9f, 0);
        guideRotation = Quaternion.LookRotation(relativePos, transform.up);


        accSpeed += accelleration * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, guideRotation, 45 * Time.deltaTime);
        rb.velocity = transform.forward * (missileSpeed + accSpeed);


        targetOld = target.position;


    }


    // ???????????? ???????????? ???????????????
    // ?????? ????????? ????????? ????????? ??????????????? ?????? ???

    [SerializeField] GameObject explosionFactory;
    private void OnCollisionEnter(Collision collision)
    {
        if (onDestroyed != null)
        {
            onDestroyed(target.gameObject);
        }

        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = collision.transform.position;


        Destroy(collision.gameObject);
        Destroy(gameObject);
    }



}