using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using System;
using Assets.code;

public class RESTServiceController : MonoBehaviour
{
    private static string googleApiKey = "AIzaSyCLi-ps7SPuoG5UAOLsElu_QhD11K-l0xw";
    private Vector3 userWorldCoords;

    public Vector3 getUserElevation(Vector3 location)
    {
        userWorldCoords.x = location.x;
        userWorldCoords.z = location.z;

        StartCoroutine(getGoogleElevation());
        location.y = userWorldCoords.y;
        //Debug.Log("getElevation(Vector3 location): LAT: " + location.z + " LON: " + location.x + " ALT: " + location.y);
        
        return userWorldCoords;
    }

    // set elevation to marker.worldCoords variable
    public IEnumerator setMarkerElevation(GameObject marker)
    {
        //Debug.Log("SENDING REST REQUEST");
        string url = "https://maps.googleapis.com/maps/api/elevation/json?locations="
            + marker.GetComponent<Marker>().worldCoords.z.ToString()
            + "," + marker.GetComponent<Marker>().worldCoords.x.ToString()
            + "&key=" + googleApiKey;
        //Debug.Log("URL: " + url);
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log(www.text);
            marker.GetComponent<Marker>().worldCoords.y = JSONDecodeGoogleElevation(www.text);
        }
        else
            Debug.Log(www.error);
        //Debug.Log("ALTITUDE FOR MARKER " + marker.GetComponent<Marker>().text + " is " + marker.GetComponent<Marker>().worldCoords.y.ToString());
    }
    
    // set elevation to worldCords Vector3 variable
    IEnumerator getGoogleElevation()
    {
        string url = "https://maps.googleapis.com/maps/api/elevation/json?locations=" 
            + userWorldCoords.z.ToString() + "," + userWorldCoords.x.ToString() 
            + "&key=" + googleApiKey;
        //Debug.Log("URL: " + url);
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log(www.text);
            userWorldCoords.y = JSONDecodeGoogleElevation(www.text);
        }
        else
            Debug.Log(www.error);
    }
    
    private static float JSONDecodeGoogleElevation(string wwwResponse)
    {
        JSONObject j = new JSONObject(wwwResponse);
        if(j["status"].ToString().Contains("OK"))
        {
            return (int)(float.Parse(j[0][0]["elevation"].ToString()));
        }

        Debug.LogError("CANT DECODE MESSAGE WITH CONTENT: " + wwwResponse);
        return float.MinValue;
    }
}