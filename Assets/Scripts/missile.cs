using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public float speed = 10;
    public float acceleration = 10;
    public float acSpeed;
    public GameObject explosionFactory;
    Transform target;
    Vector3 targetDir;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        acSpeed += acceleration * Time.deltaTime;
        if(PlayerMove.instance != null)
        {
        target = PlayerMove.instance.transform;
        }
        if (target)
        {
        targetDir = target.position - transform.position;
        // transform.rotation = new Quaternion(targetDir.x, targetDir.y, targetDir.z,1);
        dir = targetDir.normalized;
        transform.forward = targetDir.normalized;
        
        }
        else
        {
            dir = transform.forward;
        }
        transform.position += dir * (speed + acSpeed) * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = collision.transform.position;
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
