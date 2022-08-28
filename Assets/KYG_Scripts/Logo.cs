using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Logo : MonoBehaviour
{
    public GameObject StartBtn;
    public GameObject LogoSpace;
    public Image LogoImage;
    bool isStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        StartBtn.SetActive(false);
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            StartCoroutine("FadeIn");
            isStarted = true;
        }
    }
    
    public IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / 6)
        {
            LogoImage.color = new Color(1, 1, 1, i);
            if (i >= 0.99f)
            {
                StartBtn.SetActive(true);
            }
            yield return null;
        }
    }
}
