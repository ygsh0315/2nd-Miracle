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
    [SerializeField] AirplaneController controller;
    [SerializeField] float engineStartUpTime;
    public AudioSource flare;
    public AudioSource GLOC;
    public AudioSource gun;




    [SerializeField] AudioSource startSource;
    [SerializeField] public  AudioSource moveSource;
    [SerializeField] AudioSource attackSource;
    [SerializeField] AudioSource AfterBurnerSource;
    public enum MoveState
    { 
        Idle,
        Normal,
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
    public bool boostTurn;
    float curTime;
    float waitTime;
    [SerializeField] float soundPitch_min=0.7f;
    [SerializeField] float soundPitch_max=1.6f;

    void Start()
    {
        flare = gameObject.AddComponent<AudioSource>();
        GLOC= gameObject.AddComponent<AudioSource>();
        moveSource.volume = 0;
        AfterBurnerSource.volume = 0;
    }
    public  void Statemachine()
    {
        switch (moveState)
        {
            case MoveState.Idle:
                EngineStart();
                break;
            case MoveState.Normal:
                engineNormal();
                AfterBurner(0.002f);
                break;
            case MoveState.Explosion:
                moveSource.clip = audioClips[9];
                break;
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


    void EngineStart()
    {
        startSource.clip = audioClips[0];
        waitTime += Time.deltaTime;
        if (waitTime > engineStartUpTime)
        {
            moveSource.clip = audioClips[1];
            moveSource.pitch = 0.7f;
            if (!moveSource.isPlaying)
            {
                moveSource.Play();
            }
            startSource.volume -= 0.001f;
            moveSource.volume += 0.001f;
            if (moveSource.volume >= 1)
            {
                moveSource.volume = 1;
                startSource.Stop();
                controller.isStart = true;
                moveState = MoveState.Normal;

            }
        }
        if (!startSource.isPlaying)
        {
            startSource.Play();
        }
    }
    private void engineNormal()
    {
        if (controller.isWEP)
        {
            if(controller.isground)
            moveSource.pitch = soundPitch_min + (soundPitch_max - soundPitch_min) * controller.tp * 0.2f;
            else 
            {
                moveSource.pitch = soundPitch_min + (soundPitch_max - soundPitch_min) * controller.tp * 0.5f;
            }
        }
        else
        {
            moveSource.pitch = soundPitch_min + (soundPitch_max - soundPitch_min) * controller.tp ;
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

            GLOC.PlayOneShot(audioClips[5], 0.6f);
            GLOC.pitch = 0.6f;

            turn = true;
        }
    }
    void AfterBurner(float multi)
    {

        if (boostTurn)
        {
            AfterBurnerSource.clip = audioClips[3];
            AfterBurnerSource.Play();
            AfterBurnerSource.volume += multi;
            if (AfterBurnerSource.volume > 0.9f)
            {
                AfterBurnerSource.volume =0.9f;
            }
            AfterBurnerSource.loop = true;
        }
        else
        {
            AfterBurnerSource.volume -= multi;
            if (AfterBurnerSource.volume < 0)
            {
                AfterBurnerSource.Stop();
            }
            
        }
    }
    

}
