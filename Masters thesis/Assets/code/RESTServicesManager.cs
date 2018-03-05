using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using System;

 
public class RESTServicesManager : MonoBehaviour
{
    private float lat, lon, alt = 0;

    public float getElevation(float lat, float lon)
    {
        this.lat = lat;
        this.lon = lon;
        StartCoroutine(getGoogleElevation());
        return alt;
    }

    IEnumerator getGoogleElevation()
    {
        //Debug.Log("SENDING REST REQUEST");
        string url = "https://maps.googleapis.com/maps/api/elevation/json?locations=" 
            + lat.ToString() + "," + lon.ToString() 
            + "&key=AIzaSyCLi-ps7SPuoG5UAOLsElu_QhD11K-l0xw";
        //Debug.Log("URL: " + url);
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log(www.text);
            JSONDecode(www.text);
        }
        else
            Debug.Log(www.error);
    }

    private void JSONDecode(string wwwResponse)
    {
        JSONObject j = new JSONObject(wwwResponse);
        if(j["status"].ToString().Contains("OK"))
        {
            //Debug.Log("j[0][0] = " + j[0][0]["elevation"]);
            alt = (int)(float.Parse(j[0][0]["elevation"].ToString()));
        }
    }
}