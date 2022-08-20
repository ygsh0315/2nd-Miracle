
using UnityEngine;

public class M_Landing : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MissionManager.instance.isLand = true;
        }
    }
}
