using Assets.code.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    public static SettingsController Instance { get; set; }

    public GameObject popUp;
    public InputField GPSRefreshFreq, MapsRefreshFreq;
    public Slider mapsZoomSlider;
    public Toggle debugToggle;

    public bool isDisplayed = false;

    private void Awake()
    {
        Instance = this;
        popUp.SetActive(isDisplayed);
        mapsZoomSlider.onValueChanged.AddListener(onMapZoomChange);
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

    public void onMapZoomChange(float value)
    {
        // Debug.Log("zoom value: " + value);
        GoogleMapsController.Instance.zoom = (int)value;

    }

    private void loadSettings()
    {
        debugToggle.isOn = UpdateGpsText.enabled = (InternalDataController.loadValue(InternalDataController.developmentSetting) == "True") ? true : false;

        GPSRefreshFreq.text = InternalDataController.loadValue(InternalDataController.GPSRefreshFreqSetting);
        MapsRefreshFreq.text = InternalDataController.loadValue(InternalDataController.mapsRefreshFreqSetting);
        mapsZoomSlider.value = float.Parse(InternalDataController.loadValue(InternalDataController.mapsZoomLevelSetting));

        GPSController.Instance.refreshInterval = float.Parse(GPSRefreshFreq.text);
        GoogleMapsController.Instance.refreshInterval = int.Parse(MapsRefreshFreq.text);
        GoogleMapsController.Instance.zoom = (int)mapsZoomSlider.value;
    }

    private void saveSettings()
    {
        UpdateGpsText.enabled = debugToggle.isOn;
        GPSController.Instance.refreshInterval = float.Parse(GPSRefreshFreq.text);

        InternalDataController.saveValue(InternalDataController.mapsRefreshFreqSetting, MapsRefreshFreq.text);
        InternalDataController.saveValue(InternalDataController.mapsZoomLevelSetting, mapsZoomSlider.value.ToString());
        InternalDataController.saveValue(InternalDataController.developmentSetting, debugToggle.isOn.ToString());
        InternalDataController.saveValue(InternalDataController.GPSRefreshFreqSetting, GPSRefreshFreq.text);
    }

}
