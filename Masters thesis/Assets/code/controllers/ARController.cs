using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARController : MonoBehaviour {
    private Gyroscope gyroscope;
    private GameObject cameraContainer;
    private Quaternion rotation;

    private bool camAvailable = false;
    private WebCamTexture backCamera;
    //private Texture defaultBG;

    public RawImage bg;
    public AspectRatioFitter aspFitter;
    public static Quaternion deviceOrientation;

    private bool gyroAvailable = false;

	// Use this for initialization
	void Start () {
		if(!SystemInfo.supportsGyroscope)
        {
            Debug.LogError("NO GYROSCOPE FOUND");
            return;
        }

        //defaultBG = bg.texture;
        WebCamDevice[] cameras = WebCamTexture.devices;

        if (cameras.Length == 0)
        {
            Debug.LogError("NO CAMERA FOUND!");
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            if (!cameras[i].isFrontFacing) // we do not want front facing camera
            {
                backCamera = new WebCamTexture(cameras[i].name, Screen.width, Screen.height);
            }
        }

        if (backCamera == null)
        {
            Debug.LogError("NO BACK CAMERA FOUND!");
            return;
        }

        cameraContainer = new GameObject("Camera container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        backCamera.Play();
        bg.texture = backCamera;
        camAvailable = true;

        gyroscope = Input.gyro;
        gyroscope.enabled = true;
        gyroAvailable = true;
        cameraContainer.transform.rotation = Quaternion.Euler(90, 0, 0);
        rotation = new Quaternion(0, 0, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(gyroAvailable)
        {
            transform.localRotation = deviceOrientation = gyroscope.attitude * rotation;
        }

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
