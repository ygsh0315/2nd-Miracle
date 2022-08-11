<<<<<<< HEAD
using System.Collections;
=======
using System;
>>>>>>> main
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
<<<<<<< HEAD
=======
        CHAN_Missile cm;
        GameObject target;
        public Action<GameObject> onDestroyed;
        public GameObject explosionFactory;
>>>>>>> main
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            dir = transform.forward;
<<<<<<< HEAD
            player = GameObject.Find("Plane");
            rb.velocity = transform.forward * velocity;
=======
            player = GameObject.Find("Player");
            rb.velocity = transform.forward * velocity;
            cm = player.GetComponent<CHAN_Missile>();
            
>>>>>>> main
        }

        // Update is called once per frame
        void Update()
        {


        }
<<<<<<< HEAD

        private void OnTriggerEnter(Collider other)
        {
            cg = player.GetComponent<CHAN_Gun>();
            cg.bulletPool.Add(gameObject);
            gameObject.SetActive(false);
            print("들어감");
=======
        public void RemoveTarget(GameObject t)
        {
            cm.target.Remove(t);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains("Enemy"))
            {
                GameObject explosion = Instantiate(explosionFactory);
                explosion.transform.position = other.transform.position;
                RemoveTarget(other.gameObject);
                other.gameObject.GetComponent<Enemy>().hp--;
                
            }
            print("Hit");

>>>>>>> main
        }
    }
}
