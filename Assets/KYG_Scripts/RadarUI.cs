using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadarUI : MonoBehaviour
{
    public static RadarUI Instance;
    public GameObject Player;
    public Image PlayerLocation;
    public Image EnemyLocation;
    public List<GameObject> DetectedEnemyList = new List<GameObject>();
    public List<Image> EnemyLocationGroup = new List<Image>();
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLocation.transform.rotation = Quaternion.Euler(0, 0, -Player.transform.rotation.eulerAngles.y);
        //foreach(GameObject Enemy in DetectedEnemyList)
        //{
        //    if (!Enemy.gameObject)
        //    {
        //        Destroy(EnemyLocationGroup[DetectedEnemyList.IndexOf(Enemy)]);
        //        EnemyLocationGroup.RemoveAt(DetectedEnemyList.IndexOf(Enemy));
        //        DetectedEnemyList.RemoveAt(DetectedEnemyList.IndexOf(Enemy));
        //    }
        //}
        for(int i = DetectedEnemyList.Count-1; i>=0; i--)
        {
            if (!DetectedEnemyList[i])
            {
                Destroy(EnemyLocationGroup[i]);
                EnemyLocationGroup.RemoveAt(i);
                DetectedEnemyList.RemoveAt(i);
            }
        }
        if (EnemyLocationGroup.Count < DetectedEnemyList.Count)
        {
            Image enemyLocation = Instantiate(EnemyLocation);
            EnemyLocationGroup.Add(enemyLocation);
            enemyLocation.transform.SetParent(this.transform.GetChild(0));            
        }
        for(int i = 0; i<EnemyLocationGroup.Count; i++)
        {
            EnemyLocationGroup[i].transform.position = PlayerLocation.transform.position + new Vector3((Player.transform.position- DetectedEnemyList[i].transform.position ).x * -0.05f, (DetectedEnemyList[i].transform.position - Player.transform.position).z * 0.05f, 0);
            if (EnemyLocationGroup[i].transform.position.x>200 || EnemyLocationGroup[i].transform.position.y>200)
            {
                EnemyLocationGroup[i].GetComponent<Image>().enabled = false;
            }
            else
            {
                EnemyLocationGroup[i].GetComponent<Image>().enabled = true;
            }
        }
        
    }
}
