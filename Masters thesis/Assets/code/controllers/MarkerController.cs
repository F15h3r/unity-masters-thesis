using Assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.code.models;
using System;

public class MarkerController : MonoBehaviour {
    public static MarkerController Instance { get; set; }
    public static Vector3 markerScale;
    public GameObject markerPrefab, parentObject;
    private GameObject markerPrefabClone;
    private GoogleAltitudeController rsc;
    public List<GameObject> markers;

    public float refreshInterval = 1;
    private float timeSinceLastRefresh = 0;

    private void Awake()
    {
        Instance = this;
        markerScale = new Vector3(15000, 1, 15000); // TODO: read from userPrefs
        markers = new List<GameObject>();
        rsc = gameObject.AddComponent(typeof(GoogleAltitudeController)) as GoogleAltitudeController;
    }

    private void Start()
    {
        readMarkersFromMemory();
    }

    internal void remove3DMarkerInstance(MarkerData markerData)
    {
        for (int i = markers.Count - 1; i >= 0; i--)
        {
            if(markers[i].GetComponent<Marker>().data == markerData)
            {
                DestroyObject(markers[i].gameObject);
                markers.RemoveAt(i);
            }
        }
    }
    
    internal void hideMarker(MarkerData markerData)
    {
        for (int i = markers.Count - 1; i >= 0; i--)
        {
            Vector3 m = markers[i].GetComponent<Marker>().data.worldCoords;
            if (m == markerData.worldCoords)
            {
                Debug.Log("Hidding marker: " + markerData.text);
                markerData.visible = false;
                markers[i].GetComponent<Marker>().gameObject.SetActive(false);

            }
        }
    }

    private void readMarkersFromMemory() // TODO: ACTUALLY READ MARKERS FROM MEMORY...
    {
        Vector3 lisbonCenter = new Vector3();
        lisbonCenter.z = 38.713889f;
        lisbonCenter.x = -9.139444f;
        lisbonCenter.y = float.MinValue;
        create3DMarkerInstance(lisbonCenter, "Lisbon center");


        Vector3 lisbonAirport = new Vector3();
        lisbonAirport.z = 38.7755936f;
        lisbonAirport.x = -9.1268856f;
        lisbonAirport.y = float.MinValue;
        create3DMarkerInstance(lisbonAirport, "Lisbon airport");

        // 38.7365463,-9.1552203 Gulbenkian

        Vector3 gulbenkian = new Vector3();
        gulbenkian.z = 38.7365463f;
        gulbenkian.x = -9.1552203f;
        gulbenkian.y = float.MinValue;
        create3DMarkerInstance(gulbenkian, "Gulbenkian");

        Vector3 isel = new Vector3();
        isel.z = 38.7567672f;
        isel.x = -9.1167117f;
        isel.y = float.MinValue;
        create3DMarkerInstance(isel, "ISEL");
    }

    public void create3DMarkerInstance(Vector3 worldCoords, string markerText)
    {
        markerPrefabClone =
            Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;

        markerPrefabClone.GetComponent<Marker>().Setup(worldCoords, markerText);
        if (worldCoords.y != float.MinValue)
            StartCoroutine(rsc.setMarkerElevation(markerPrefabClone));

        markers.Add(markerPrefabClone);
    }

    void Update () {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshInterval)
        {
            timeSinceLastRefresh = 0;

            update3DMarkers();
        }
    }

    private void update3DMarkers()
    {
        foreach (GameObject marker in markers)
        {
            if (marker.GetComponent<Marker>().data.visible)
            {
                marker.GetComponent<Marker>().setRelativeGamePosition();
                marker.transform.LookAt(new Vector3(0, -10, 0));
            }
        }
    }
    /*
    private void debugMarkersOutput()
    {
        string s = parentObject.transform.childCount + " markerjev na obzorju:\n";
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            s += i + ". :" + parentObject.transform.GetChild(i).name + "\n";
        }

        Debug.Log(s);
    }
    */
}
