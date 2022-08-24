using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_pass : MonoBehaviour
{
    //한번만 작동되도록 해야 한다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MissionManager.instance.missionCount++;
            Destroy(gameObject);
        }
    }
}
