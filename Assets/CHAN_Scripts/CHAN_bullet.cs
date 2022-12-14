using System;
using System.Collections.Generic;
using UnityEngine;
namespace MFlight
{
    public class CHAN_bullet : MonoBehaviour
    {
        float defaultSpeed;
        //[SerializeField]float speed;
        [SerializeField] float velocity = 30;

        Rigidbody rb;
        Vector3 dir;
        CHAN_Gun cg;
        GameObject player;
        CHAN_Missile cm;
        GameObject target;
        public Action<GameObject> onDestroyed;
        public GameObject explosionFactory;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            dir = transform.forward;
            player = GameObject.Find("Player");
            rb.velocity = transform.forward * velocity;
            cm = player.GetComponent<CHAN_Missile>();
            
        }

        // Update is called once per frame
        void Update()
        {


        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains("Enemy"))
            {
                GameObject explosion = Instantiate(explosionFactory);
                explosion.transform.position = other.transform.position;
                other.gameObject.GetComponent<Enemy>().hp--;
                
            }
        }
    }
}
