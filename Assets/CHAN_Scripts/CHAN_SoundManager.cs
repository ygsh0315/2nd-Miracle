using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_SoundManager : MonoBehaviour
{
    public static CHAN_SoundManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    [SerializeField] public AudioClip[] audioClips = null;
    public AudioSource boost;
    public AudioSource flare;
    public AudioSource GLOC;
    public AudioSource gun;


    AudioSource moveSource;
    [SerializeField] AudioSource attackSource;
    public enum MoveState
    { 
        Idle,
        Normal,
        AfterBurner,
        Explosion,
        Gun,
        GLOC,
        Warning
    }
    public MoveState moveState;
    public enum AttackState
    { 
        Idle,
        gun,
        seeking,
        Lock
    }
    public AttackState attackState;
    bool turn;
    float curTime;
    void Start()
    {
        moveSource = gameObject.AddComponent<AudioSource>();
        boost = gameObject.AddComponent<AudioSource>();
        flare = gameObject.AddComponent<AudioSource>();
        GLOC= gameObject.AddComponent<AudioSource>();
        attackSource = gameObject.AddComponent<AudioSource>();
    }
    public  void Statemachine()
    {
        switch (moveState)
        {
            case MoveState.Idle:
                moveSource.clip = audioClips[0];
                break;
            case MoveState.Normal:
                moveSource.clip = audioClips[1];
                break;
            case MoveState.AfterBurner:
                moveSource.clip = audioClips[2];
                moveSource.loop = true;
                break;
            case MoveState.Explosion:
                moveSource.clip = audioClips[9];
                break;
        }
        if (!moveSource.isPlaying)
        { 
            moveSource.Play();  
        }

        switch (attackState)
        {
            case AttackState.Idle:
                attackSource.Stop();
                break;
            case AttackState.gun:
                attackSource.clip = audioClips[6];
                break;
            case AttackState.seeking:
                attackSource.clip = audioClips[7];
                break;
            case AttackState.Lock:
                attackSource.clip = audioClips[8];
                break;
        }
        if (!attackSource.isPlaying)
        {
            attackSource.Play();
        }
    }
    void Update()
    {
        Statemachine();
    }

    public void Gloc()
    {
        curTime += Time.deltaTime;
        if (curTime > 3)
        {
            turn = false;
            curTime = 0;
        }
        if (!turn)
        {
            flare.PlayOneShot(audioClips[5], 1);
            turn = true;
        }
    }
    

}
