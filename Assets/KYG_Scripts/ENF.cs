using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENF : MonoBehaviour
{
    Transform ELCS;
    public GameObject explosionFactory;
    // Start is called before the first frame update
    void Start()
    {
        ELCS = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
