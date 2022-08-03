using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���� �����Ǹ� �����Ѵ�
//�ʿ�Ӽ�: ���� ��ǥ, ���� ����


public class Turret : MonoBehaviour
{
    //������
    public float DetectingAltitude = 5f;
    //��������
    public float DetectingRange = 100f;
    //��������
    bool isDetected = false;
    //�ͷ�����
    public state turretState;
    //�̻��ϰ���
    public GameObject missileFactory;
    //�̻������
    public List<GameObject> missilePool = new List<GameObject>();
    //���
    public int missilePoolSize = 12;
    //�̻��� �߻� ����
    public float fireDelay = 2f;
    //����ð�
    float currentTime;

    //�߻���ġ
    Transform firePosition;
    //ȸ����
    Transform launcher;
    //�߻��
    Transform holder;
    //Ÿ��
    Transform target;
    //Ÿ�ٹ���
    Vector3 targetDir;
    //Ÿ�ٰ��� �Ÿ�
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
        //���������Ͻ� ȸ���ϰ� �ʹ�.
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
