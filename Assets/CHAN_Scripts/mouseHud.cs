using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseHud : MonoBehaviour
{
    [Header("Components")]
        [SerializeField] private mouseFlightController mouseFlight = null;

        [Header("HUD Elements")]
        [SerializeField] private RectTransform boresight = null;
        [SerializeField] private RectTransform mousePos = null;

        private Camera playerCam = null;
    private void Awake()
    {
        playerCam=mouseFlight.GetComponentInChildren<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseFlight == null || playerCam == null)
                return;

            UpdateGraphics(mouseFlight);
    }
    void UpdateGraphics(mouseFlightController controller) 
    {
        if(boresight!=null)
        {
            boresight.position=playerCam.WorldToScreenPoint(controller.BoresightPos);
            boresight.gameObject.SetActive(boresight.position.z > 1f);
        }
         if(mousePos!=null)
        {
            mousePos.position=playerCam.WorldToScreenPoint(controller.MouseAimPos);
            mousePos.gameObject.SetActive(mousePos.position.z>1f);
        }
            
        
    }
}
