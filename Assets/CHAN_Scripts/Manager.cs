using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject turret_pure;
    public GameObject turret_lead;

    void Start()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].SetActive(false);

        }
        turret_pure.SetActive(false);
        turret_lead.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            print("�� ����");
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            print("pure �� ����");
            turret_pure.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            print("lead �� ����");
            turret_lead.SetActive(true);
        }
    }
}
