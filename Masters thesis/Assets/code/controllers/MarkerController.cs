using Assets.code;
using System.Collections.Generic;
using UnityEngine;
using Assets.code.models;
using System;
using Assets.code.controllers;

public class MarkerController : MonoBehaviour {
    public static MarkerController Instance { get; set; }
    public Vector3 unityWorldRatio;
    public GameObject markerPrefab, parentObject;
    private GameObject markerPrefabClone;
    private GoogleAltitudeController rsc;
    public List<GameObject> markers;


    public float refreshInterval = 0.1f;
    private float timeSinceLastRefresh = 0;

    private void Awake()
    {
        Instance = this;
        unityWorldRatio = new Vector3(0.1f, 0.1f, 0.1f);
        markers = new List<GameObject>();
        rsc = gameObject.AddComponent(typeof(GoogleAltitudeController)) as GoogleAltitudeController;
    }

    private void Start()
    {
        InternalDataController.readMarkersFromMemory();
    }


    void Update () {
        timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (timeSinceLastRefresh >= refreshInterval)
        {
            timeSinceLastRefresh = 0;

            
        }

        updateVisible3DMarkers();

    }

    private void updateVisible3DMarkers()
    {
        foreach (GameObject marker in markers)
        {
            if (marker.GetComponent<Marker>().data.visible)
            {
                marker.GetComponent<Marker>().setRelativeGamePosition();
                marker.transform.LookAt(Application.userGamePosition);
            }
        }
    }

    #region markers adding / removing

    internal void remove3DMarkerInstance(MarkerData markerData, bool removeFromMarkersList = true)
    {
        for (int i = markers.Count - 1; i >= 0; i--)
        {
            if(markers[i].GetComponent<Marker>().data == markerData)
            {
                DestroyObject(markers[i].gameObject);
                if(removeFromMarkersList)
                    markers.RemoveAt(i);
            }
        }
    }

    public void add3DMarkerInstance(Vector3 worldCoords, string markerName, string description)
    {

        markerPrefabClone =
            Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;

        markerPrefabClone.GetComponent<Marker>().Setup(worldCoords, markerName, description);

        if (worldCoords.y == float.MinValue)
            StartCoroutine(rsc.setMarkerElevation(markerPrefabClone));

        markers.Add(markerPrefabClone);
    }
    
    public void toggle3DMarkerVisibility(MarkerData markerData)
    {
        markerData.visible = !markerData.visible;

        for (int i = markers.Count - 1; i >= 0; i--)
        {
            if (markers[i].GetComponent<Marker>().data == markerData)
            {
                markers[i].gameObject.SetActive(markerData.visible);
            }
        }
    }

    #endregion


    #region save / load from device memory


    public void load3DMarkerInstance(MarkerData markerData)
    {
        markerPrefabClone =
            Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;
        markerPrefabClone.GetComponent<Marker>().loadFromMarkerData(markerData);

        if (markerData.worldCoords.y == float.MinValue)
            StartCoroutine(rsc.setMarkerElevation(markerPrefabClone));

        markerPrefabClone.SetActive(markerData.visible);
        markers.Add(markerPrefabClone);
    }

    public void OnApplicationPause(bool pause)
    {
        if(pause)
            InternalDataController.saveSerializedMarkersList();
    }

    public void OnApplicationQuit()
    {
        InternalDataController.saveSerializedMarkersList();
    }

    #endregion
}
