using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어의 체력을 관리하고 싶다.
//필요속성 : 체력
public class PlayerHP : MonoBehaviour
{
    public GameObject explosionFactory;
    [SerializeField] CHAN_PlayerEffectManager effect;
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
                effect.StartDie();  
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
