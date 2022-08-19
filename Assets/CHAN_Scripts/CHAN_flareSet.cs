using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_flareSet : MonoBehaviour
{
    //플레어 발사되면 플레어가 나아가도록 만든다.

    [SerializeField] List<GameObject> flarePool = new List<GameObject>();
    [SerializeField] CHAN_SoundManager sound;

    public Transform[] flarespot;
    public GameObject flareFac;
    float curTime;
    [SerializeField] float setTime = 0.2f;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject flare = Instantiate(flareFac);
            flarePool.Add(flare);
            flare.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            
            curTime += Time.deltaTime;
            if (curTime > setTime)
            {
                sound.flare.PlayOneShot(sound.audioClips[4], 1);
                for (int j = 0; j < 2; j++)
                {
                    
                    flarePool[j].SetActive(true);
                    flarePool[j].transform.position = flarespot[j].transform.position;
                    flarePool[j].transform.forward = flarespot[j].transform.forward;
                    flarePool.Remove(flarePool[j]);
                    curTime = 0;
                }
            }
        }
    }

}
