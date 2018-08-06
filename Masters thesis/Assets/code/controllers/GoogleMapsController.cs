using Assets.code;
using Assets.code.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoogleMapsController : MonoBehaviour {
    public static GoogleMapsController Instance { get; set; }
    private string url;
    private int Zoom = 14;
    public int zoom
    {
        set
        {
            if (value >= 10 && value <= 20)
                Zoom = value;

            reloadMapImage();
        }

        get { return Zoom; }
    }
    private int width = 640;
    private int height = 640;
    private float downloadTime = 0;

    private enum MapType { roadmap, satellite, hybrid, terrain };
    private MapType selectedMapType = MapType.roadmap;
    private int scale = 2; // scale 2 sets returning image resolution of 1280x1280

    public RawImage image;
    private string mapStyle = "&style=feature:administrative.land_parcel%7Cvisibility:off&style=feature:administrative.neighborhood%7Cvisibility:off&style=feature:poi.business%7Cvisibility:off&style=feature:poi.park%7Celement:labels.text%7Cvisibility:off&style=feature:road%7Celement:labels%7Cvisibility:off&style=feature:water%7Celement:labels.text%7Cvisibility:off";
    private Vector3 lastUserWorldLocation;
    public float refreshInterval = 0;
    private float timeSinceLastRefresh = 10;

    void Awake()
    {
        Instance = this;

        if (PlayerPrefs.HasKey(InternalDataController.mapsRefreshFreqSetting))
            refreshInterval = float.Parse(InternalDataController.loadValue(InternalDataController.mapsRefreshFreqSetting));

        if (PlayerPrefs.HasKey(InternalDataController.mapsZoomLevelSetting))
            zoom = int.Parse(InternalDataController.loadValue(InternalDataController.mapsZoomLevelSetting));
        
    }

    IEnumerator getMap()
    {
        // Google maps static map api: https://developers.google.com/maps/documentation/static-maps/intro
        // Google maps styling wizard: https://mapstyle.withgoogle.com/

        url = "https://maps.googleapis.com/maps/api/staticmap?center="
            + GPSController.Instance.userWorldLocation.z + ","
            + GPSController.Instance.userWorldLocation.x
            + "&zoom=" + Zoom
            + "&size=" + width + "x" + height
            + "&scale=" + scale
            + "&maptype=" + selectedMapType
            + "&key=" + Application.googleApiKey
            + mapStyle;

        foreach(GameObject go in MarkerController.Instance.markers)
        {
            if(go.GetComponent<Marker>().data.visible)
                url += "&markers=color:red%7Clabel:M%7C" + go.GetComponent<Marker>().data.worldCoords.z + "," + go.GetComponent<Marker>().data.worldCoords.x;
        }

        if (GPSController.Instance.userLocationStable)
        {

            wwwController wCtrl = gameObject.AddComponent<wwwController>();
            yield return StartCoroutine(wCtrl.wwwRequest(url));
            image.texture = wCtrl.www.texture;
            Destroy(wCtrl);

        }

        yield return null;
    }

    void Update()
    {
        
        if(refreshInterval == 0)
        {
            if (GPSController.Instance.userLocationStable)
                if (GPSController.Instance.userWorldLocation != lastUserWorldLocation) // Reload map on user move
                {
                    reloadMapImage();
                    lastUserWorldLocation = GPSController.Instance.userWorldLocation;
                }

        }
        else
        {
            timeSinceLastRefresh += Time.unscaledDeltaTime;

            if (timeSinceLastRefresh >= refreshInterval)
            {
                reloadMapImage();
            }
        }

    }

    /*
     * Re-download map image from google servers for current user location
     */
    public void reloadMapImage()
    {
        Debug.Log("RELOADING MAP IMAGE");
        timeSinceLastRefresh = 0;
        StopAllCoroutines();
        StartCoroutine(getMap());
    }
}
