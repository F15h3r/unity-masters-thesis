using Assets.code.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    public static SettingsController Instance { get; set; }

    public GameObject popUp;
    public InputField GPSRefreshFreq;
    public Toggle debugToggle;

    public bool isDisplayed = false;

    private void Awake()
    {
        Instance = this;
        popUp.SetActive(isDisplayed);
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
        debugToggle.isOn = UpdateGpsText.enabled = (InternalDataController.loadValue(InternalDataController.developmentSetting) == "True") ? true : false;

        GPSRefreshFreq.text = InternalDataController.loadValue(InternalDataController.GPSRefreshFreqSetting);
        GPSController.Instance.refreshInterval = float.Parse(GPSRefreshFreq.text);


    }

    private void saveSettings()
    {
        UpdateGpsText.enabled = debugToggle.isOn;
        GPSController.Instance.refreshInterval = float.Parse(GPSRefreshFreq.text);

        InternalDataController.saveValue(InternalDataController.developmentSetting, debugToggle.isOn.ToString());
        InternalDataController.saveValue(InternalDataController.GPSRefreshFreqSetting, GPSRefreshFreq.text);
    }

}
