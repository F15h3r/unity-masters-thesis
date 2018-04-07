using Assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerController : MonoBehaviour {
    public static MarkerController Instance { get; set; }
    public static Vector3 markerScale;
    public GameObject markerPrefab, parentObject;
    private GameObject markerPrefabClone;
    private RESTServiceController rsc;
    private List<GameObject> markers;
    private int framesToReload = 150;

    private void Start()
    {
        DontDestroyOnLoad(this);
        markerScale = new Vector3(15000, 1, 15000); // TODO: read from userPrefs
        markers = new List<GameObject>();
        rsc = gameObject.AddComponent(typeof(RESTServiceController)) as RESTServiceController;

        //showMarkerFromPrefab(new Vector3(100, 100, 100), "100, 100, 100");
        //showMarkerFromPrefab(new Vector3(-100, -100, -100), "-100, -100, -100");
        Vector3 lisbonCenter = new Vector3();
        lisbonCenter.z = 38.713889f;
        lisbonCenter.x = -9.139444f;
        lisbonCenter.y = float.MinValue;
        showMarker(lisbonCenter,  "Lisbon center");

        Vector3 lisbonAirport = new Vector3();
        lisbonAirport.z = 38.7755936f;
        lisbonAirport.x = -9.1268856f;
        lisbonAirport.y = float.MinValue;
        showMarker(lisbonAirport, "Lisbon airport");

        // 38.7365463,-9.1552203 Gulbenkian

        Vector3 gulbenkian = new Vector3();
        gulbenkian.z = 38.7365463f;
        gulbenkian.x = -9.1552203f;
        gulbenkian.y = float.MinValue;
        showMarker(gulbenkian, "Gulbenkian");

        Vector3 isel = new Vector3();
        isel.z = 38.7567672f;
        isel.x = -9.1167117f;
        isel.y = float.MinValue;
        showMarker(isel, "ISEL");

    }

    private void showMarker(Vector3 worldCoords, string markerText)
    {
        markerPrefabClone = 
            Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;

        markers.Add(markerPrefabClone);
        markerPrefabClone.GetComponent<Marker>().setLatitude(worldCoords.z);
        markerPrefabClone.GetComponent<Marker>().setLongitude(worldCoords.x);
        markerPrefabClone.GetComponent<Marker>().text = markerText;

        StartCoroutine(rsc.setMarkerElevation(markerPrefabClone));

        markerPrefabClone.GetComponent<Marker>().setRelativeGamePosition();
        markerPrefabClone.transform.Rotate(new Vector3(0, 180, 0)); // Rotate the prefab
        markerPrefabClone.transform.LookAt(new Vector3(0, -20, 0)); // Look at the camera


    }

    void Update () {
        if(framesToReload < 0)
        {
            //Debug.Log("UPDATE MARKERS");
            framesToReload = 150;
            foreach(GameObject marker in markers)
            {
                marker.GetComponent<Marker>().setRelativeGamePosition();
                marker.transform.LookAt(new Vector3(0, -20, 0)); // Look at the camera
                //Debug.Log(marker.GetComponent<Marker>().text + " ALT: " + marker.GetComponent<Marker>().worldCoords.y.ToString() + " GAME ALT: " + marker.GetComponent<Marker>().transform.position.y.ToString());
            }
        }
        else
            framesToReload--;

    }
}
