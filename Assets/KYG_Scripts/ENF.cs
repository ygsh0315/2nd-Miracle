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

    //여기서 플레이어 미사일에게 격추됐을때 발동되도록 만든다.
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
            //또다른 폭발효과
            // 카메라 쉐이킹
            // 폭발소리
            
        }
    }

}
