using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGpsText : MonoBehaviour {
    public Text textToDisplay;
    private int frames, FPS;
    private float FPSTimer;

    private void Start()
    {
        FPSTimer = 1f;
    }

    private void Update ()
    {
        FPSUpdate();

        textToDisplay.text = 
              "LAT: " + GPSController.Instance.userWorldLocation.z + "\n"
            + "LON: " + GPSController.Instance.userWorldLocation.x + "\n"
            + "ALT: " + GPSController.Instance.userWorldLocation.y + "\n" 
            + "ORIENT: " + MarklessAR.deviceOrientation.ToString() + "\n"
            + "FPS: " + FPS;
    }

    private void FPSUpdate()
    {
        frames++;
        FPSTimer -= Time.unscaledDeltaTime;
        if (FPSTimer <= 0f)
        {
            FPS = frames;
            FPSTimer = 1f;
            frames = 0;
        }
    }
}
