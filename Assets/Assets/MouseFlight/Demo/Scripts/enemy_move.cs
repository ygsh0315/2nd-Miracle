using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_move : MonoBehaviour
{
    //그저 적이 이리저리 움직이게 하는 코드
    [SerializeField] float speed = 10f;
    float curTime;
    Vector3 dir;
    float multi = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > 10)
        {
            multi *= -1;
            curTime = 0;
        }
        dir = transform.forward * multi;
        transform.position += transform.forward * speed * Time.deltaTime;
        // if(curTime>0.1f)
        // {
        //     transform.Rotate(-1,0,0);
        //     curTime=0;
        // }

    }
}
