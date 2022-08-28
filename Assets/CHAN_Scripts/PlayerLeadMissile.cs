using System;
using System.Collections.Generic;
using UnityEngine;

//���� �����ϴ� �̻���

public class PlayerLeadMissile : MonoBehaviour
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

    Transform ELCS;
    CHAN_Missile cm;
    bool isClose = false;

    public GameObject explosionFactory;
    AudioSource audio;
    [SerializeField]AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        cm = player.GetComponent<CHAN_Missile>();
        target = cm.detected[0];
        ELCS = target.transform.GetChild(0);
        audio = GetComponent<AudioSource>();
        audio.clip = audioClips[0];
        audio.Play();
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
        //if (target)
        //{
        //    if (distance < 10)
        //    {
        //        isClose = true;
        //        dir = transform.forward;
        //    }
        //}

        transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.position += transform.forward.normalized * LMspeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = collision.transform.position;
        audio.PlayOneShot(audioClips[1], 1);
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().hp--;
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<ENF>())
        {
            collision.gameObject.GetComponent<ENF>().isTargetHit = true;
            
            Destroy(gameObject);
        }
        // 현재 지형지물까지 없어지는 이슈 발생
        // 지형지물 닿으면 그냥 자폭하게 둚
        // 하지만 목표물, 적 비행기 내부의 ELCS 가 접촉하도록만들어야 함
        //접촉한 물체가 지형지물 일 경우 자폭
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            print("지형닿음");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Destroy(gameObject);
        }

    }
}
