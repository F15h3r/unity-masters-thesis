using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GPS : MonoBehaviour {
    public static GPS Instance { set; get; }

    public float latitude, longitude, altitude;
    private int maxAttempts = 20;
    private int numFramesToRefreshLocation = 150;
    private RESTServicesManager restManager;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
        //restManager = new RESTServicesManager();
        restManager = gameObject.AddComponent(typeof(RESTServicesManager)) as RESTServicesManager;
    }

    private void Update()
    {
        if (numFramesToRefreshLocation < 0)
        {
            Debug.Log("UPDATING LOCATION NOW!");
            numFramesToRefreshLocation = 300;
            StartCoroutine(StartLocationService());
        }
        else
        {
            numFramesToRefreshLocation--;
        }
    }

    private IEnumerator StartLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("USER GAVE NO PERMISSION TO ACCESS GPS");
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
            Debug.Log("TIMED OUT: CAN'T DETERMINE LOCATION");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GPS ERROR");
            yield break;
        }

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        //altitude = Input.location.lastData.altitude;  // this altitude is not good
        altitude = restManager.getElevation(latitude, longitude);
        /*
        if(altitude == 0)
        {
            Debug.Log("GET ALTITUDE FROM GOOGLE");
            altitude = restManager.getElevation(latitude, longitude);
        }
        */
        yield break;
    }
}
