using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_pass : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            MissionManager.instance.missionCount++;
        }
    }
}
