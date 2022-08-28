using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMissileIcon : MonoBehaviour
{
    [SerializeField] private GameObject targetIcon = null;
    [SerializeField] GameObject missile;
     GameObject player;
    GameObject icon;
    Vector3 dir;
    float angle;
    bool isBehind;

    void Start()
    {
        icon = Instantiate(targetIcon, transform);
        icon.SetActive(true);
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dir = (missile.transform.position - Camera.main.transform.position).normalized;
        angle=Vector3.Angle(Camera.main.transform.forward, dir);
        if (angle > 90)
        {
            isBehind = true;
        }
        else
        {
            isBehind = false;
        }
        if (!isBehind)
        {
            icon.transform.position = Camera.main.WorldToScreenPoint(missile.transform.position) + new Vector3(10, 20, 0);
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }
        
    }
    private void OnDestroy()
    {
        icon.SetActive(false);
    }
}
