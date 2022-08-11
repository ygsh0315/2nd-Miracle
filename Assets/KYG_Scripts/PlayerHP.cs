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
                GameObject explosion = Instantiate(explosionFactory);
                explosion.transform.position = transform.position;
                Destroy(gameObject);
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
