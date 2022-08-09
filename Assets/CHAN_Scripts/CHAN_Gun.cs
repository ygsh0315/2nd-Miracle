using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFlight
{
public class CHAN_Gun : MonoBehaviour
{
   [SerializeField] private MouseFlightController mouseFlight = null;
   [SerializeField] GameObject bulletFac;
   [SerializeField] Transform firePos;
   [SerializeField] public List<GameObject> bulletPool = new List<GameObject>();
   [SerializeField] int ammo=500;
   [SerializeField] float multifly=0.01f;
   float curTime;
   [SerializeField] float delayTime=0.1f;
   
    void Start()
    {
        for(int i=0;i<ammo;i++)
        {
            GameObject bullet = Instantiate(bulletFac);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        curTime+=Time.deltaTime;
        if(Input.GetButton("Fire1"))
        {
            float x = Random.Range(-10,10)*multifly;
            float y = Random.Range(-10,10)*multifly;
            Vector3 dir= transform.right*x+transform.up*y;
            
            if(curTime>delayTime)
            {
                GameObject bullet =bulletPool[0];
                bullet.SetActive(true);
                bulletPool.Remove(bullet);
                bullet.transform.position=firePos.position;
                //여기서 랜덤요소를 추가한다
                bullet.transform.forward=transform.forward+dir;
                curTime=0;
            }
        }
    }
}
}
