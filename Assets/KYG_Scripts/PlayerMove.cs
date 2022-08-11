using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 100;
    public Transform Target;
    public static PlayerMove instance;
    //sphere collider À§Ä¡
    public GameObject sc;
    public float acc = 30;
    public float accSpeed = 0;
    public float maxSpeed = 100;
    public float finalSpeed = 0;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        accSpeed += acc * Time.deltaTime;
        finalSpeed = accSpeed + speed;
        if (finalSpeed > maxSpeed)
        {
            finalSpeed = maxSpeed;
        }
        sc.transform.position = transform.position + transform.forward.normalized * finalSpeed * 0.01f;   
        sc.GetComponent<SphereCollider>().radius = LeadMissile.LMspeed * 0.01f;       
        //transform.RotateAround(Target.position, Vector3.up, speed * Time.deltaTime);
        transform.position += transform.forward * finalSpeed * Time.deltaTime;
    }
}
