using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LaderUI : MonoBehaviour
{
    public Image PlayerLocation;
    public GameObject Player;
    public List<Image> EnemyLocationGroup = new List<Image>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLocation.transform.rotation = Quaternion.Euler(0, 0, -Player.transform.rotation.eulerAngles.y);
    }
}
