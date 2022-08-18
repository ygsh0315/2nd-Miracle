
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    
    public float detectRange = 2000;
    public LayerMask Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Detact();
    }

    private void Detact()
    {
        Collider[] detectedEnemy = Physics.OverlapSphere(transform.position, detectRange,Enemy);
        foreach(Collider Enemy in detectedEnemy)
        {
            print(detectedEnemy.Length);
            print(detectedEnemy[0].name);
            if (!RadarUI.Instance.DetectedEnemyList.Contains(Enemy.transform.gameObject))
            {
                RadarUI.Instance.DetectedEnemyList.Add(Enemy.transform.gameObject);
            }
            
        }
    }
}
