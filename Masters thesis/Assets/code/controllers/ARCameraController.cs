using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARCameraController : MonoBehaviour {
    public static ARCameraController Instance { get; set; }
    private GameObject cameraContainer; // empty game object to rotate camera view

    private bool backCameraAvailable = false;
    private WebCamTexture backCamera;
    public Camera mainCamera;
    public RawImage bg;
    public AspectRatioFitter aspFitter;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {

        WebCamDevice[] cameras = WebCamTexture.devices;

        if (cameras.Length == 0)
        {
            Debug.LogError("NO CAMERA FOUND!");
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            if (!cameras[i].isFrontFacing) // no front cameras!
            {
                backCamera = new WebCamTexture(cameras[i].name, Screen.width, Screen.height);
            }
        }

        if (backCamera == null)
        {
            Debug.LogError("NO BACK CAMERA FOUND!");
            return;
        }

        cameraContainer = new GameObject("Main Cam container");
        cameraContainer.transform.position = mainCamera.transform.position;
        mainCamera.transform.SetParent(cameraContainer.transform);
        cameraContainer.transform.rotation = Quaternion.Euler(90, 0, 0);

        backCamera.Play();
        bg.texture = backCamera;
        backCameraAvailable = true;
	}
	
	
	void Update () {
        if (GyroController.Instance.gyroAvailable && backCameraAvailable)
        {
            mainCamera.transform.localRotation = GyroController.Instance.deviceOrientation;
        }
        else return;

        float ratio = (float)backCamera.width / (float)backCamera.height;
        aspFitter.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        bg.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orientation = -backCamera.videoRotationAngle;
        bg.rectTransform.localEulerAngles = new Vector3(0f, 0f, orientation);
    }
}
