﻿using Assets.code.controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GPSController : MonoBehaviour {
    public static GPSController Instance { set; get; }
    public Vector3 userWorldLocation;
    public bool userLocationStable = false;
    private GoogleAltitudeController rsc;
    private int maxAttempts = 20;
    public float refreshInterval = 5;
    private float timeSinceLastRefresh = 0;

    private void Awake()
    {
        Instance = this;
        userWorldLocation = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        rsc = gameObject.AddComponent(typeof(GoogleAltitudeController)) as GoogleAltitudeController;

        if(PlayerPrefs.HasKey(InternalDataController.GPSRefreshFreqSetting))
            refreshInterval = float.Parse(InternalDataController.loadValue(InternalDataController.GPSRefreshFreqSetting));
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(startLocationService()); 

        //userWorldLocation.x = 0;
        //userWorldLocation.z = 38.7302485f;
        //userLocationStable = true;
    }

    private void Update()
    {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshInterval)
        {
            timeSinceLastRefresh = 0;

            if (userLocationStable)
            {
                userWorldLocation.z = Input.location.lastData.latitude;
                userWorldLocation.x = Input.location.lastData.longitude;
                userWorldLocation.y = rsc.getUserElevation(userWorldLocation).y;
            }
            else StartCoroutine(startLocationService());

            
        }
    }

    private IEnumerator startLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            locationWarning("USER GAVE NO PERMISSION TO ACCESS GPS");

            yield break;
        }

        maxAttempts = 20;

        Input.location.Start();

        while(Input.location.status == LocationServiceStatus.Initializing && maxAttempts > 0)
        {
            yield return new WaitForSeconds(1);
            maxAttempts--;
        }

        if(maxAttempts <= 0)
        {
            locationWarning("TIMED OUT: CAN'T DETERMINE LOCATION");

            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            locationWarning("UNKNOWN GPS ERROR");

            yield break;
        }

        if (Input.location.lastData.latitude != 0 && Input.location.lastData.longitude != 0)
        {
            userWorldLocation.z = Input.location.lastData.latitude;
            userWorldLocation.x = Input.location.lastData.longitude;
            userWorldLocation.y = rsc.getUserElevation(userWorldLocation).y;
            userLocationStable = true;
        }
        else
        {
            locationWarning("location NOT found! atempts: " + (20 - maxAttempts) + " current location reading = lat:"
                + Input.location.lastData.latitude +" lon:"+ Input.location.lastData.longitude);
        }

        yield break;
    }

    private void locationWarning(string msg)
    {
        Debug.LogWarning(msg);
        userLocationStable = false;
        userWorldLocation = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
}
