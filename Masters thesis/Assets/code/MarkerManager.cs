using Assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour {
    public static MarkerManager Instance { get; set; }
    private List<GameObject> markers;
    public GameObject markerPrefab, parentObject;

    private void Start()
    {
        markers = new List<GameObject>();
        GameObject markerPrefabClone = Instantiate(markerPrefab, transform.position, Quaternion.identity, parentObject.transform) as GameObject;
        markers.Add(markerPrefabClone);

        Debug.Log("st markerjev: " + markers.Count);
        Debug.Log("Markers Parent: " + markers[0].transform.parent.name);
        //markers[0].GetComponent<Marker>().tag = "test";
        markers[0].transform.position = new Vector3(-100, -100, -100);
        //markers[0].GetComponentsInChildren; // TODO: access marker->Button->Text element and change it
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("st markerjev: " + POIs.Count);
	}
}
