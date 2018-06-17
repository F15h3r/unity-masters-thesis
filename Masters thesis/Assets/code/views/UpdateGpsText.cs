using Assets.code.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGpsText : MonoBehaviour {
    public static bool enabled = false;
    public Text textToDisplay;
    private int frames, FPS;
    private float FPSTimer;

    private void Awake()
    {
        if(PlayerPrefs.HasKey(InternalDataController.GPSRefreshFreqSetting))
            enabled = (InternalDataController.loadValue(InternalDataController.developmentSetting) == "True") ? true : false;

        FPSTimer = 1f;
    }

    private void Update ()
    {
        if (enabled)
        {
            FPSUpdate();

            textToDisplay.text =
                  "LAT: " + GPSController.Instance.userWorldLocation.z + "\n"
                + "LON: " + GPSController.Instance.userWorldLocation.x + "\n"
                + "ALT: " + GPSController.Instance.userWorldLocation.y + "\n"
                + "ORIENT: " + GyroController.Instance.deviceOrientation.ToString() + "\n"
                + "FPS: " + FPS;
        }
        else textToDisplay.text = "";
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
