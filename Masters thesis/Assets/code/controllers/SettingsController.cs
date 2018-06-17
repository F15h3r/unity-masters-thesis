using Assets.code.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    public static SettingsController Instance { get; set; }
    public static string developmentSetting = "devON", 
        GPSRefreshFreqSetting = "gpsRefFreq", 
        googleMapUpdateSetting = "googlMapsFreq";
    public GameObject popUp;
    public InputField GPSRefreshFreq;
    public Toggle debugToggle;

    public bool isDisplayed = false;

    private void Awake()
    {
        Instance = this;
        popUp.SetActive(isDisplayed);
        loadSettings();
    }

    public void toggleSettingsPopup()
    {
        isDisplayed = !isDisplayed;
        popUp.SetActive(isDisplayed);

        if (isDisplayed)
        {
            loadSettings();
        }
        else
        {
            saveSettings();
        }
    }

    private void loadSettings()
    {
        debugToggle.isOn = (InternalDataController.loadValue(developmentSetting) == "True") ? true : false;
        GPSRefreshFreq.text = InternalDataController.loadValue(GPSRefreshFreqSetting);
    }

    private void saveSettings()
    {
        InternalDataController.saveValue(developmentSetting, debugToggle.isOn.ToString());
        InternalDataController.saveValue(GPSRefreshFreqSetting, GPSRefreshFreq.text);
        InternalDataController.saveValue(googleMapUpdateSetting, googleMapUpdateSetting.ToString());
    }

}
