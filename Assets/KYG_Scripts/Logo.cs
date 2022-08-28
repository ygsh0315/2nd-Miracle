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
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            StartCoroutine("FadeOut");
            isStarted = true;
        }
    }
    public IEnumerator FadeOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / 7)
        {
            LogoImage.color = new Color(1, 1, 1, i);
            if (i > 0.9f)
            {
                StartBtn.SetActive(true);
            }
            yield return null;
        }
    }
}
