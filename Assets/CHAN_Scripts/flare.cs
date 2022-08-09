using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flare : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody prb;
    [SerializeField] float scale = 120;
    [SerializeField] float vel = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * vel, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector3.down * scale, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("missile"))
        {
            Destroy(other.gameObject);
        }
    }
}
