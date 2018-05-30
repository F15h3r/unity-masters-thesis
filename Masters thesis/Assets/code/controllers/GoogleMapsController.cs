using Assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapsController : MonoBehaviour {
    public static GoogleMapsController Instance { get; set; }
    private string url;
    private int zoom = 13;
    private int width = 640;
    private int height = 640;
    private float downloadTime = 0;

    private enum MapType { roadmap, satellite, hybrid, terrain };
    private MapType selectedMapType = MapType.roadmap;
    private int scale = 2; // scale 2 makes returning images 1280x1280

    public RawImage image;
    private string mapStyle = "&style=feature:administrative.land_parcel%7Cvisibility:off&style=feature:administrative.neighborhood%7Cvisibility:off&style=feature:poi.business%7Cvisibility:off&style=feature:poi.park%7Celement:labels.text%7Cvisibility:off&style=feature:road%7Celement:labels%7Cvisibility:off&style=feature:water%7Celement:labels.text%7Cvisibility:off";
    //private string mapStyle = "&maptype=roadmap"; // TODO: PREKLOPI NA ZGORNJEGA KO MARKERJI DELUJEJO
    private static string googleApiKey = "AIzaSyApdbgfaSwua1iDr0dsAl3v4I6ZJ4Emc9c";

    public float refreshInterval = 15;
    private float timeSinceLastRefresh = 8;

    IEnumerator getMap()
    {
        // Google maps static map api: https://developers.google.com/maps/documentation/static-maps/intro
        // Google maps styling wizard: https://mapstyle.withgoogle.com/

        url = "https://maps.googleapis.com/maps/api/staticmap?center="
            + GPSController.Instance.userWorldLocation.z + ","
            + GPSController.Instance.userWorldLocation.x
            + "&zoom=" + zoom
            + "&size=" + width + "x" + height
            + "&scale=" + scale
            + "&maptype=" + selectedMapType
            + "&key=" + googleApiKey
            + mapStyle;

        foreach(GameObject go in MarkerController.Instance.markers)
        {
            if(go.GetComponent<Marker>().data.visible)
                url += "&markers=color:red%7Clabel:M%7C" + go.GetComponent<Marker>().data.worldCoords.z + "," + go.GetComponent<Marker>().data.worldCoords.x;
        }

        if (GPSController.Instance.userLocationStable)
        {
            WWW www = new WWW(url);
            downloadTime = 0;

            while (!www.isDone)
            {
                downloadTime += Time.deltaTime;
                if (downloadTime >= 10.0f) break;
                // print("GMaps DL: " + www.progress);
                yield return null;
            }

            if (!www.isDone || !string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Load Failed");
                yield break;
            }

            image.texture = www.texture;
            Debug.Log("Map image w:" + www.texture.width + " h:" + www.texture.height);

        }
    }

	void Start () {
        Instance = this;
	}

    void Update()
    {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshInterval)
        {
            reloadMapImage();
        }
    }

    /*
     * Re-download map image from google servers for current user location
     */
    public void reloadMapImage()
    {
        timeSinceLastRefresh = 0;
        StartCoroutine(getMap());
    }
}
