
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Enemy FSM
    public enum EnemyState
    {
        Detact,
        Attack,
        Idle,
        Avoid,
        Destroy
    }

    public enum AvoidState
    {
        EnemyAvoid,
        EnvironmentAvoid
    }
    public EnemyState state = EnemyState.Idle;
    #endregion
    public GameObject target;

    public GameObject ENF;

    public float speed = 100;

    public float detactRange = 1000;

    public float missileRange = 500;

    public float attackRange = 1000;

    public float fireRange = 100;

    public float avoidRange = 100;


    #region missile

    //�̻��� ��ź��
    public int missilePoolSize = 4;
    //�̻��� �߻� ����
    public int missileDelay = 2;

    //�̻��� ����
    public float missileSpacing = 2f;

    //�̻��� �ð�
    public float missileCurrentTime = 0;


    //�̻��ϰ���
    public GameObject missileFactory;

    //�̻��� �߻���ġ

    public GameObject missileFirePosition;
    //�̻��� �߻� ��ġ ����
    public GameObject missileFirePositionFactory;

    //�̻��� źâ
    List<GameObject> missileFirePositions = new List<GameObject>();

    //�̻������
    public List<GameObject> missilePool = new List<GameObject>();
    #endregion

    #region bullet
    //�Ѿ˰���
    public GameObject bulletFactory;
    //�Ѿ����
    public List<GameObject> bulletPool = new List<GameObject>();

    //�Ѿ� �߻���ġ
    public Transform firePosition;
    //�Ѿ� ��ź��
    public int bulletPoolSize = 100;
    //�Ѿ� �߻� ����
    public float bulletDelay = 0.1f;


    float bulletCurrentTime = 0;
    #endregion


    float distance;

    Vector3 dir;

    public float idleTime = 10f;

    float currentTime = 0;

    float randomDirTime = 3f;

    public GameObject sc;

    public GameObject FlightEffect;

    public GameObject DestroyEffect;

    public int hp = 1;

    Ray detectRay;

    RaycastHit EnvironmentInfo;

    LayerMask layerMask;

    Rigidbody rb;

    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        target = GameObject.Find("Player");
        sc.transform.position = transform.position + transform.forward.normalized * speed * 0.01f;
        sc.GetComponent<SphereCollider>().radius = LeadMissile.LMspeed * 0.01f;
        rb = GetComponent<Rigidbody>();
        DestroyEffect.SetActive(false);
        for (int i = 0; i < missilePoolSize; i++)
        {
            GameObject missileFirePosition = Instantiate(missileFirePositionFactory);
            missileFirePosition.transform.parent = transform.Find("MissilePools").transform;
            missileFirePositions.Add(missileFirePosition);
            missileFirePosition.SetActive(true);
            float totalSpacing = missilePoolSize * missileSpacing;
            Vector3 firstPos = transform.position + new Vector3(-(totalSpacing - 1.5f) * 0.5f, -0.5f, 1.5f);
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            state = EnemyState.Destroy;
          
        }
        
        //print(Vector3.Angle(transform.forward, target.transform.position) < 30f);
        //print(Vector3.Angle(transform.forward, target.transform.position));
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
            case EnemyState.Destroy:
                Destroy();
                break;
        }
        if (target && state != EnemyState.Idle && state != EnemyState.Destroy)
        {
            distance = (target.transform.position - transform.position).magnitude;  
            transform.forward = Vector3.Lerp(transform.forward, dir, 1 * Time.deltaTime);
            //transform.position += transform.forward * speed * Time.deltaTime;
            rb.velocity = transform.forward * speed;
        }
        if (hp <= 0)
        {
            state = EnemyState.Destroy;
        }
        layerMask = 1 << 4 | 1 << 10;
        detectRay = new Ray(transform.position, transform.forward);
        Physics.Raycast(detectRay, out EnvironmentInfo,detactRange, layerMask);
        //Debug.DrawRay(transform.position, transform.forward * avoidRange, Color.red);
    }

    
    private void Idle()
    {

        if (currentTime > idleTime || !ENF)
        {
            state = EnemyState.Detact;
        }
    }

    private void Detact()
    {     
        if (distance < detactRange)
        {
            dir = (target.transform.position - transform.position).normalized;
        }
        if (EnvironmentInfo.transform && EnvironmentInfo.distance < avoidRange)
        {
            state = EnemyState.Avoid;
        }
        if (distance < attackRange)
        {
            state = EnemyState.Attack;         
        }
        
    }
    
    private void Attack()
    {
        if (distance < avoidRange)
        {
            state = EnemyState.Avoid;
        }
        if(EnvironmentInfo.transform&& EnvironmentInfo.distance < avoidRange)
        {
            state = EnemyState.Avoid;
        }
        if (target)
        {
            dir = (target.transform.position - transform.position).normalized;
        }
        if (distance < missileRange && Vector3.Angle(transform.forward, target.transform.position-transform.position) < 30f &&missilePool.Count>0)
        {
                FireMissile();
        }
        if (bulletPool.Count > 0 && target && distance < fireRange && Vector3.Angle(transform.forward, target.transform.position - transform.position) < 15f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        bulletCurrentTime += Time.deltaTime;
        if ( bulletCurrentTime > bulletDelay)
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
        missileCurrentTime += Time.deltaTime;      

        
        for (int i = missilePool.Count-1; i>=0; i--)
        {
            if (missileCurrentTime > missileDelay)
            {
                missilePool.RemoveAt(i);
                missileFirePosition.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                missileFirePosition.transform.GetChild(i).GetChild(0).GetComponent<LeadMissile>().enabled = true;
                missileFirePosition.transform.GetChild(i).GetChild(0).GetComponent<Rigidbody>().isKinematic = false;               
                missileFirePosition.transform.GetChild(i).GetChild(0).transform.parent = null;
                missileCurrentTime = 0;
            }
        }

    }
    
    private void Avoid()
    {
        if (EnvironmentInfo.transform && EnvironmentInfo.distance < avoidRange)
        {
            dir = SetDir();
        //print("WHY");
        //    for (int i = 0; i < avoidRange; i++)
        //    {
        //        for(int j = 0; j<avoidRange; j++)
        //        {
                    
        //            Ray avoidRay = new Ray(transform.position, transform.position + new Vector3(j, i, avoidRange));
        //            Debug.DrawRay(transform.position, new Vector3(j, i, avoidRange) * avoidRange, Color.red);
        //            Physics.Raycast(avoidRay, out avoidInfo, 10, layerMask);
        //            if (!avoidInfo.transform)
        //            {
        //                dir = new Vector3(j, i, avoidRange).normalized;
        //            }
        //        }
        //        for (int k = 0; k > -avoidRange; k--)
        //        {

        //            Ray avoidRay = new Ray(transform.position, transform.position + new Vector3(k, i, avoidRange));
        //            Debug.DrawRay(transform.position, new Vector3(k, i, avoidRange) * avoidRange, Color.red);
        //            Physics.Raycast(avoidRay, out avoidInfo, 10, layerMask);
        //            if (!avoidInfo.transform)
        //            {
        //                dir = new Vector3(k, i, avoidRange).normalized;
        //            }
        //        }
        //        if (!avoidInfo.transform)
        //        {
        //            break;
        //        }
        //    }
            
        }
        else
        {
            print("this");
            if (currentTime > randomDirTime)
            {
                dir = Random.insideUnitSphere.normalized;
                currentTime = 0;
            }
        }
        print(!avoidInfo.transform && !EnvironmentInfo.transform && distance > avoidRange * 5);
        //print(avoidInfo.transform.gameObject.name);
        //print(EnvironmentInfo.transform.gameObject.name);
        if (!avoidInfo.transform && !EnvironmentInfo.transform && distance > avoidRange * 5)
        {
            state = EnemyState.Detact;
        }
    }
    RaycastHit avoidInfo;
    public Vector3 SetDir()
    {
        Vector3 avoidDir = Vector3.zero;
        Ray avoidDetectRay;
        for(int i = 0; i<avoidRange; i++)
        {
            for(int j = 0; j<avoidRange; j++)
            {
                avoidDetectRay = new Ray(transform.position, transform.forward + new Vector3(j, i, avoidRange));
                Physics.Raycast(avoidDetectRay, out avoidInfo, avoidRange, layerMask);
                Debug.DrawRay(transform.position, transform.forward + new Vector3(j, i, avoidRange), Color.red);
                if (!avoidInfo.transform)
                {
                    avoidDir = transform.forward + new Vector3(j, i, avoidRange);
                    break;
                }
            }
            for(int k = 0; k>-avoidRange; k--)
            {
                avoidDetectRay = new Ray(transform.position, transform.forward + new Vector3(k, i, transform.position.z + avoidRange));
                Physics.Raycast(avoidDetectRay, out avoidInfo, avoidRange, layerMask);
                Debug.DrawRay(transform.position, transform.forward + new Vector3(k, i, avoidRange), Color.red);
                if (!avoidInfo.transform)
                {
                    avoidDir = transform.forward + new Vector3(k, i, avoidRange);
                    break;
                }
            }
        }
        return avoidDir;
    }
    private void Destroy()
    {
        if(FlightEffect.activeSelf == false)
        {
            return;
        }
        audio.Play();
        FlightEffect.SetActive(false);
        DestroyEffect.SetActive(true);
        rb.AddForce(transform.forward * speed * 100);
        rb.useGravity = true;
        Destroy(gameObject, 15f);
    }



}
