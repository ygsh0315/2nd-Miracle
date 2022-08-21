using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�÷��̾��� ü���� �����ϰ� �ʹ�.
//�ʿ�Ӽ� : ü��
public class PlayerHP : MonoBehaviour
{
    public GameObject explosionFactory;
    public int hp = 10;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if(hp<=0)
            {
                gameObject.GetComponent<AirplaneController>().canControl = false;
               
            }
        }
    }
    public static PlayerHP Instance = null;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


}
