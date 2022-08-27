using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileWarning : MonoBehaviour
{
    [SerializeField] Collider[] enemyMissiles;
    [SerializeField] Collider[] missileClose;
    float[] distance; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        enemyMissiles = Physics.OverlapSphere(transform.position, 1000, 1 << 8);
        missileClose = Physics.OverlapSphere(transform.position, 300, 1 << 8);
        if (enemyMissiles.Length > 0)
        {
            CHAN_SoundManager.instance.missileAlarmSource.clip = CHAN_SoundManager.instance.audioClips[10];
            if (missileClose.Length > 0)
            {
                CHAN_SoundManager.instance.missileAlarmSource.clip = CHAN_SoundManager.instance.audioClips[11];
            }
        }
        if (!CHAN_SoundManager.instance.missileAlarmSource.isPlaying)
        {
            CHAN_SoundManager.instance.missileAlarmSource.Play();
        }


    }
}
