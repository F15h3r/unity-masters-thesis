using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceCamera : MonoBehaviour {
    private bool camAvailable;
    private WebCamTexture backCamera;
    private Texture defaultBG;

    public RawImage bg;
    public AspectRatioFitter aspFitter;

    private void Start()
    {
        defaultBG = bg.texture;
        WebCamDevice[] cameras = WebCamTexture.devices;

        if(cameras.Length == 0)
        {
            Debug.Log("NO CAMERA FOUND!");
        }

        for(int i=0; i<cameras.Length; i++)
        {
            if(!cameras[i].isFrontFacing) // we do not want front facing camera
            {
                backCamera = new WebCamTexture(cameras[i].name, Screen.width, Screen.height);
            }
        }

        if(backCamera == null)
        {
            Debug.Log("NO BACK CAMERA FOUND!");
            return;
        }

        backCamera.Play();
        bg.texture = backCamera;
        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCamera.width / (float)backCamera.height;
        aspFitter.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        bg.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orientation = -backCamera.videoRotationAngle;
        bg.rectTransform.localEulerAngles = new Vector3(0f, 0f, orientation);
    }
    
}
