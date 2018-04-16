using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapsController : MonoBehaviour {

    private string url;
    public int zoom = 15;
    public int width = 640;
    public int height = 640;

    public enum MapType { roadmap, satellite, hybrid, terrain };
    public MapType selectedMapType = MapType.roadmap;
    public int scale = 1;

    private RawImage image;
    private string mapStyle = "&maptype=roadmap&style=feature:administrative.land_parcel%7Cvisibility:off&style=feature:administrative.neighborhood%7Cvisibility:off&style=feature:poi.business%7Cvisibility:off&style=feature:poi.park%7Celement:labels.text%7Cvisibility:off&style=feature:road%7Celement:labels%7Cvisibility:off&style=feature:water%7Celement:labels.text%7Cvisibility:off";
    private static string googleApiKey = "AIzaSyApdbgfaSwua1iDr0dsAl3v4I6ZJ4Emc9c";

    public float refreshInterval = 15;
    private float timeSinceLastRefresh = 10;

    IEnumerator Map()
    {
        //Google maps static map api: https://developers.google.com/maps/documentation/static-maps/intro
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

            //+ "&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=YourAPIKeyWillbeHere";
        WWW www = new WWW(url);
        yield return www;


        image.texture = www.texture;
        image.SetNativeSize();
        Debug.Log("Map image w:" + image.texture.width + " h:" + image.texture.height);
    }

	void Start () {
        image = gameObject.GetComponent<RawImage>();
	}

    void Update()
    {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshInterval)
        {
            //Debug.Log("Request map image now");
            timeSinceLastRefresh = 0;
            if(GPSController.Instance.userLocationStable)
                StartCoroutine(Map());
        }
    }
}
