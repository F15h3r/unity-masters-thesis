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
    private int numFramesToRefreshLocation = 150;

    private void Awake()
    {
        Instance = this;
        userWorldLocation = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        rsc = gameObject.AddComponent(typeof(RESTServiceController)) as RESTServiceController;        
    }

    private void Start()
    {

        //DontDestroyOnLoad(gameObject);
        StartCoroutine(startLocationService());
    }

    private void Update()
    {
        if (numFramesToRefreshLocation < 0)
        {
            //Debug.Log("UPDATE USER LOCATION NOW!");
            numFramesToRefreshLocation = 300;
            StartCoroutine(startLocationService());
        }
        else
        {
            numFramesToRefreshLocation--;
        }
    }

    private IEnumerator startLocationService()
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

        userWorldLocation.z = Input.location.lastData.latitude;
        userWorldLocation.x = Input.location.lastData.longitude;
        //altitude = Input.location.lastData.altitude;  // this altitude is not good

        userWorldLocation.y = rsc.getUserElevation(userWorldLocation).y;

        if (userWorldLocation.z != float.MinValue && userWorldLocation.x != float.MinValue)
            userLocationStable = true;

        yield break;
    }
}
