using Assets.code;
using System.Collections.Generic;
using UnityEngine;
using Assets.code.models;
using System;

public class MarkerController : MonoBehaviour {
    public static MarkerController Instance { get; set; }
    public static Vector3 markerScale;
    public GameObject markerPrefab, parentObject;
    private GameObject markerPrefabClone;
    private GoogleAltitudeController rsc;
    private const string markersDataPlayerPrefs = "markersData";
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

    public void add3DMarkerInstance(Vector3 worldCoords, string markerName, string description)
    {

        markerPrefabClone =
            Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;

        markerPrefabClone.GetComponent<Marker>().Setup(worldCoords, markerName, description);

        if (worldCoords.y == float.MinValue)
            StartCoroutine(rsc.setMarkerElevation(markerPrefabClone));

        markers.Add(markerPrefabClone);
    }

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

    public void set3DMarkerVisible(MarkerData markerData)
    {
        for (int i = markers.Count - 1; i >= 0; i--)
        {
            if (markers[i].GetComponent<Marker>().data == markerData)
            {
                markers[i].GetComponent<Marker>().data.visible = true;
            }
        }
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

    private void readMarkersFromMemory()
    {
        if (PlayerPrefs.HasKey(markersDataPlayerPrefs))
        {
            string jsonMarkersData = PlayerPrefs.GetString(markersDataPlayerPrefs);
            MarkersDataList mdl = new MarkersDataList();

            try
            {
                JsonUtility.FromJsonOverwrite(jsonMarkersData, mdl);

                foreach (MarkerData md in mdl.markersDataList)
                {
                    load3DMarkerInstance(md);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Deserialization error: " + e.ToString());
            }

        }
        else
        {
            Debug.LogError("Markers data playerPrefs doesn't exist!");
        }
    }

    private string serializeMarkers()
    {
        string dataToSave = "{\"markersDataList\":[";
        if(markers.Count > 0)
        {
            foreach (GameObject go in markers)
            {
                dataToSave += JsonUtility.ToJson(go.GetComponent<Marker>().data) + ",";
            }
            dataToSave = dataToSave.Substring(0, dataToSave.Length - 1) + "]}";
        }
        else
        {
            dataToSave += "]}";
        }

        return dataToSave;
    }

    private void saveSerializedMarkersList()
    {
        PlayerPrefs.SetString(markersDataPlayerPrefs, serializeMarkers());
        PlayerPrefs.Save();
    }

    public void OnApplicationPause(bool pause)
    {
        if(pause)
            saveSerializedMarkersList();
    }

    public void OnApplicationQuit()
    {
        saveSerializedMarkersList();
    }

    // TODO: REMOVE THIS
    private void addDummyMarkers()
    {
        
        Vector3 lisbonCenter = new Vector3();
        lisbonCenter.z = 38.713889f;
        lisbonCenter.x = -9.139444f;
        lisbonCenter.y = float.MinValue;
        add3DMarkerInstance(lisbonCenter, "Lisbon center (ROSSIO)", "Famous Rossio square.");


        Vector3 lisbonAirport = new Vector3();
        lisbonAirport.z = 38.7755936f;
        lisbonAirport.x = -9.1268856f;
        lisbonAirport.y = float.MinValue;
        add3DMarkerInstance(lisbonAirport, "Lisbon airport", "Lisbon official airport offers great connectivity to other countries as well as the city center and surroundings.");

        // 38.7365463,-9.1552203 Gulbenkian

        Vector3 gulbenkian = new Vector3();
        gulbenkian.z = 38.7365463f;
        gulbenkian.x = -9.1552203f;
        gulbenkian.y = float.MinValue;
        add3DMarkerInstance(gulbenkian, "Gulbenkian", "");

        Vector3 isel = new Vector3();
        isel.z = 38.7567672f;
        isel.x = -9.1167117f;
        isel.y = float.MinValue;
        add3DMarkerInstance(isel, "ISEL", "ISEL (Instituto Superior de Engenharia de Lisboa) resulted from the restructuring of an institution with a long-standing tradition in engineering teaching in Portugal, the Industrial Institute of Lisbon (Instituto Industrial de Lisboa), which was founded in 1852 by Royal Decree signed by Queen Maria II. In 1988 it became part of a network of Polytechnic Higher Education institutions, integrated in IPL - Polytechnic Institute of Lisbon (Instituto Politécnico de Lisboa).");
        
    }
}
