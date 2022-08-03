using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//적이 감지되면 격추한다
//필요속성: 적의 좌표, 감지 범위


public class Turret : MonoBehaviour
{
    //감지고도
    public float DetectingAltitude = 5f;
    //감지범위
    public float DetectingRange = 100f;
    //감지여부
    bool isDetected = false;
    //터렛상태
    public state turretState;
    //미사일공장
    public GameObject missileFactory;
    //미사일재고
    public List<GameObject> missilePool = new List<GameObject>();
    //재고량
    public int missilePoolSize = 12;
    //미사일 발사 간격
    public float fireDelay = 2f;
    //현재시간
    float currentTime;

    //발사위치
    Transform firePosition;
    //회전대
    Transform launcher;
    //발사대
    Transform holder;
    //타겟
    Transform target;
    //타겟방향
    Vector3 targetDir;
    //타겟과의 거리
    public float targetDistance;
    public enum state
    {
        Detect,
        Attack,
        Idle
    }

    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        launcher = transform.GetChild(0);
        holder = transform.GetChild(0).GetChild(0).GetChild(0);
        firePosition = holder;
        turretState = state.Detect;

        for(int i=0; i<missilePoolSize; i++)
        {
            GameObject missile = Instantiate(missileFactory);
            missilePool.Add(missile);
            missile.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            turretState = state.Detect;
        }
        currentTime += Time.deltaTime;
        //감지상태일시 회전하고 싶다.
        if(isDetected == true)
        {
            if (target)
            {    
                targetDir = (target.position - transform.position).normalized;
            }
            launcherRotate(targetDir);
            holderRotate(targetDir);
        }
        if(turretState == state.Detect)
        {
            turretDetect();
        }
        else if(turretState == state.Attack && target)
        {
            turretAttack();
        }
        else if(turretState == state.Idle)
        {
            turretIdle();
        }
    }

    

    private void turretDetect()
    {
        if (!target)
        {
            return;
        }
        targetDistance = (target.position - transform.position).magnitude;
        if (target.position.y > DetectingAltitude && targetDistance < DetectingRange)
        {
            isDetected = true;
            turretState = state.Attack;
        }
        
    }

    private void launcherRotate(Vector3 targetDir)
    {
        Quaternion from = launcher.transform.rotation;
        Quaternion to = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));
        launcher.transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime);
    }

    private void holderRotate(Vector3 targetDir)
    {
        Quaternion from = holder.transform.rotation;
        Quaternion to = Quaternion.LookRotation(new Vector3(targetDir.x, targetDir.y, targetDir.z));
        holder.transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime);
    }

    
    private void turretAttack()
    {
        if (missilePool.Count > 0)
        {
            if (currentTime > fireDelay)
            {
                GameObject missile = missilePool[0];
                missile.SetActive(true);
                missile.transform.position = firePosition.position;
                missile.transform.forward = launcher.transform.forward;
                missilePool.RemoveAt(0);
                currentTime = 0;
            }
        }
        if (target.position.y < DetectingAltitude || targetDistance > DetectingRange)
        {
            isDetected = false;
            turretState = state.Detect;
        }
        
        if(missilePool.Count == 0)
        {
            turretState = state.Idle;
        }
    }
    private void turretIdle()
    {
        isDetected = false;
    }
}
