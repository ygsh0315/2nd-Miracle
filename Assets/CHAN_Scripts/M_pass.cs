using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_pass : MonoBehaviour
{
    //�ѹ��� �۵��ǵ��� �ؾ� �Ѵ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            Destroy(gameObject);
        }
    }
}
