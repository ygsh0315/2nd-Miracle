using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_start : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            MissionManager.instance.state = MissionManager.State.missionStart;
            print("¥Í¿Ω");
        }
    }
}
