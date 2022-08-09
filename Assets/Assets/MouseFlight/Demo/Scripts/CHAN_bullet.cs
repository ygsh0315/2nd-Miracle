using System.Collections;
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
        public GameObject explosionFactory;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            dir = transform.forward;
            player = GameObject.Find("Plane");
            rb.velocity = transform.forward * velocity;
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
                Destroy(other.gameObject);
            }
            // cg = player.GetComponent<CHAN_Gun>();
            // cg.bulletPool.Add(gameObject);
            // gameObject.SetActive(false);
            print("Hit");

        }
    }
}
