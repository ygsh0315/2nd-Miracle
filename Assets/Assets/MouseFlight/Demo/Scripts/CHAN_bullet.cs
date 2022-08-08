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

        private void OnTriggerEnter(Collider other)
        {
            cg = player.GetComponent<CHAN_Gun>();
            cg.bulletPool.Add(gameObject);
            gameObject.SetActive(false);
            print("들어감");
        }
    }
}
