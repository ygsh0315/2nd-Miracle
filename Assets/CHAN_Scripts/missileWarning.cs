using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileWarning : MonoBehaviour
{
    [SerializeField] Collider[] enemyMissiles;
    [SerializeField] Collider[] missileClose;

    AudioSource Launch;
    AudioSource Close;
    bool isLaunch;
    bool isClose;
    GameObject player;
    void Start()
    {
        Launch = CHAN_SoundManager.instance.missileAlarmSource.AddComponent<AudioSource>();
        Close = CHAN_SoundManager.instance.missileAlarmSource.AddComponent<AudioSource>();
        Launch.clip = CHAN_SoundManager.instance.audioClips[10];
        Close.clip= CHAN_SoundManager.instance.audioClips[11];
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        enemyMissiles = Physics.OverlapSphere(transform.position, 1000, 1 << 8);
        missileClose = Physics.OverlapSphere(transform.position, 100, 1 << 8);
        if (enemyMissiles.Length > 0)
        {
            isLaunch = true;
        }
        else
        {
            isLaunch = false;
        }
        if (missileClose.Length > 0)
        {
            isClose = true;
        }
        else
        {
            isClose = false;
        }
        if (player.activeSelf)
        {
            if (isLaunch && !isClose)
            {
                if (!Launch.isPlaying)
                {
                    Close.Stop();
                    Launch.Play();
                }
            }
            if (isLaunch && isClose)
            {
                if (!Close.isPlaying)
                {
                    Launch.Stop();
                    Close.Play();
                }
            }
        }
        else
        {
            print("Á¤Áö");
            Launch.Stop();
            Close.Stop();
        }
    }
}
