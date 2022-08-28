using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENF : MonoBehaviour
{
    Transform ELCS;
    public GameObject explosionFactory;
    public bool isTargetHit;
    AudioSource audio;
    [SerializeField]AudioClip clip;
    float curTime=0;
    bool turn;
    // Start is called before the first frame update
    void Start()
    {
        ELCS = transform.GetChild(0);
        audio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        DoExplosion();
    }

    //���⼭ �÷��̾� �̻��Ͽ��� ���ߵ����� �ߵ��ǵ��� �����.
    void DoExplosion()
    {
        
        if (isTargetHit)
        {
            if (!turn)
            { 
                audio.PlayOneShot(clip, 1);
                //gameObject.GetComponent<Collider>().enabled = false;
                turn= true;
            }
            curTime += Time.deltaTime;
            if (curTime > 3)
            {
                Destroy(gameObject);
            }
            //�Ǵٸ� ����ȿ��
            // ī�޶� ����ŷ
            // ���߼Ҹ�
            
        }
    }

}
