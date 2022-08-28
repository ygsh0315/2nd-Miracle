
using UnityEngine;

public class M_Landing : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        
        if (MissionManager.instance)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                MissionManager.instance.isLand = true;
            }
        }
        if (TutorialManager.instance)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                
                TutorialManager.instance.isLand = true;
            }
        }
        
    }
}
