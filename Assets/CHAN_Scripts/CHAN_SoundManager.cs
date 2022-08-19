using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_SoundManager : MonoBehaviour
{
    
    [SerializeField]public AudioClip[] audioClips=null;
    public AudioSource boost;
    public AudioSource flare;


    AudioSource moveSource;
    public enum State
    { 
        Idle,
        Normal,
        AfterBurner,
        Explosion,
        Gun,
        Seek,
        Lock,
        Launch,
        GLOC,
        Flare,
        Warning
    }
    public State state;
    void Start()
    {
        moveSource = gameObject.AddComponent<AudioSource>();
        boost = gameObject.AddComponent<AudioSource>();
        flare = gameObject.AddComponent<AudioSource>();
    }
    public  void Statemachine()
    {
        switch (state)
        {
            case State.Idle:
                moveSource.clip = audioClips[0];
                break;
            case State.Normal:
                moveSource.clip = audioClips[1];
                break;
            case State.AfterBurner:
                moveSource.clip = audioClips[2];
                moveSource.loop = true;
                break;
        }
        if (!moveSource.isPlaying)
        { 
            moveSource.Play();
            
        }
    }
    void Update()
    {
        Statemachine();
    }

    public void AfterBurner()
    {
        StartCoroutine(Boost());
    }
    public IEnumerator Boost()
    { 
        //boost.PlayOneShot(audioClips[3], 1);
        yield return new WaitForSeconds(0.5f);
        
    }

}
