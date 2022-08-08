
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 상태정의
    public enum EnemyState
    {
        Detact,
        Attack,
        Idle,
        Avoid
    }

    public EnemyState state = EnemyState.Idle;
    #endregion
    public GameObject target;
    
    public float speed = 100;

    public float detactRange = 1000;

    public float missileRange = 500;
    
    public float attackRange = 1000;
    
    public float fireRange = 100;

    public float avoidRange = 100;


    #region 미사일

    //미사일 장탄수
    public int missilePoolSize = 4;
    //미사일 발사 간격
    public float missileDelay = 0.1f;

    //미사일 간격
    public float missileSpacing = 2f;
    
    //미사일 시간
    public float missileCurrentTime = 0;
    
    
    //미사일공장
    public GameObject missileFactory;
    
    //미사일 발사위치
   
    public GameObject missileFirePosition;
    //미사일 발사 위치 공장
    public GameObject missileFirePositionFactory;
   
    //미사일 탄창
    List<GameObject> missileFirePositions = new List<GameObject>();
   
    //미사일재고
    List<GameObject> missilePool = new List<GameObject>();
    #endregion

    #region 총알
    //총알공장
    public GameObject bulletFactory;
    //총알재고
    public List<GameObject> bulletPool = new List<GameObject>();
    
    //총알 발사위치
    public Transform firePosition;
    //총알 장탄수
    public int bulletPoolSize = 100;
    //총알 발사 간격
    public float bulletDelay = 0.1f;


    float bulletCurrentTime = 0;
    #endregion
    
    
    float distance;

    Vector3 dir;

    public float idleTime = 10f;

    float currentTime = 0;

    float randomDirTime = 3f;

    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        for (int i = 0; i < missilePoolSize; i++)
        {
            GameObject missileFirePosition = Instantiate(missileFirePositionFactory);
            missileFirePosition.transform.parent = transform.Find("MissilePools").transform;
            missileFirePositions.Add(missileFirePosition);
            missileFirePosition.SetActive(true);
            float totalSpacing = missilePoolSize  * missileSpacing;
            Vector3 firstPos = transform.position +  new Vector3(-(totalSpacing-1.5f) * 0.5f, -0.5f, 1.5f);
            missileFirePositions[i].transform.position = firstPos + new Vector3(i * missileSpacing, 0, 0);
            GameObject missile = Instantiate(missileFactory);
            missilePool.Add(missile);
            missile.transform.forward = transform.forward;
            missile.transform.GetChild(0).gameObject.SetActive(false);
            missilePool[i].transform.parent = missileFirePositions[i].transform;
            missilePool[i].transform.position = missileFirePositions[i].transform.position;
            missile.GetComponent<LeadMissile>().enabled = false;
            
        }
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(Vector3.Angle(transform.forward, target.transform.position) < 30f);
        print(Vector3.Angle(transform.forward, target.transform.position));
        currentTime += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Detact:
                Detact();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Avoid:
                Avoid();
                break;
        }
        if (target && state != EnemyState.Idle)
        {
            distance = (target.transform.position - transform.position).magnitude;
            transform.forward = Vector3.Lerp(transform.forward, dir, 1 * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void Avoid()
    {
        if (currentTime > randomDirTime)
        {
            dir = Random.insideUnitSphere.normalized;
            currentTime = 0;
        }
        
        if (distance > avoidRange * 2)
        {
            state = EnemyState.Detact;
        }
    }

    private void Idle()
    {
        
        if (currentTime > idleTime)
        {
            state = EnemyState.Detact;
        }
    }

    private void Detact()
    {
        if(distance < detactRange)
        {
            dir = (target.transform.position - transform.position).normalized;
            
        }
        if (distance < attackRange)
        {
            state = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        dir = (target.transform.position - transform.position).normalized;
        if (distance < avoidRange)
        {
            state = EnemyState.Avoid;
        }
        if (distance < missileRange && Vector3.Angle(transform.forward,target.transform.position)<30f)
        {
            FireMissile();
            
        }
        if (distance < fireRange)
        {
            Fire();
        }
    }

    private void Fire()
    {
        print("Fire");
        bulletCurrentTime += Time.deltaTime;
        if(bulletPool.Count>0 && target && bulletCurrentTime > bulletDelay && distance< fireRange && Vector3.Angle(transform.forward, target.transform.position-transform.position) < 15f)
        {
            GameObject bullet = bulletPool[0];
            bullet.SetActive(true);
            bullet.transform.position = firePosition.position;
            bullet.transform.forward = transform.forward;
            bulletPool.RemoveAt(0);
            bulletCurrentTime = 0;
        }

    }

    private void FireMissile()
    {
        print("Missile");
        StartCoroutine(MissileActive());

        
    }

    IEnumerator MissileActive()
    {
        for (int i = 0; i < missilePoolSize; i++)
        {
            if (missilePool.Count>0 && target && distance < missileRange 
                && Vector3.Angle(transform.forward, target.transform.position - transform.position) < 30)
            {
                missileFirePosition.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                missileFirePosition.transform.GetChild(i).GetChild(0).GetComponent<LeadMissile>().enabled = true;
                yield return new WaitForSeconds(missileDelay);
            }
        }
    }
}
