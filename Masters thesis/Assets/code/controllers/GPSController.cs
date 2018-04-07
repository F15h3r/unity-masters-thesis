using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GPSController : MonoBehaviour {
    public static GPSController Instance { set; get; }
    public Vector3 userWorldLocation;
    public bool userLocationStable = false;
    private RESTServiceController rsc;
    private int maxAttempts = 20;
    public float refreshLocationInterval = 5;
    private float timeSinceLastRefresh = 0;

    private void Awake()
    {
        Instance = this;
        userWorldLocation = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        rsc = gameObject.AddComponent(typeof(RESTServiceController)) as RESTServiceController;        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(startLocationService());
    }

    private void Update()
    {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshLocationInterval)
        {
            //Debug.Log("UPDATE USER LOCATION NOW!");
            timeSinceLastRefresh = 0;
            StartCoroutine(startLocationService());
        }
    }

    private IEnumerator startLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            locationError("USER GAVE NO PERMISSION TO ACCESS GPS");

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
            locationError("TIMED OUT: CAN'T DETERMINE LOCATION");

            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            locationError("UNKNOWN GPS ERROR");

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
            userLocationStable = false;
            Debug.LogError("location NOT found! atempts: " + (20 - maxAttempts) + " your location: lat:"
                + Input.location.lastData.latitude +" lon:"+ Input.location.lastData.longitude);
        }

        yield break;
    }

    private void locationError(string msg)
    {
        Debug.Log(msg);
        userLocationStable = false;
        userWorldLocation = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
}
