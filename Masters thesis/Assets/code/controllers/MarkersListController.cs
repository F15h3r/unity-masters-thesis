using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersListController : MonoBehaviour {
    public GameObject markerButtonPrefab;
    private GameObject markerButtonPrefabClone;

	// Use this for initialization
	void Start () {
        refreshDisplay();
	}
	
	public void refreshDisplay()
    {
        removeAllButtons();
        addAllButtons();
    }


    private void addAllButtons()
    {
        foreach(GameObject marker in MarkerController.Instance.markers)
            addMarkerButtonToList(marker.GetComponent<Marker>().data);
    }

    private void addMarkerButtonToList(MarkerData markerData)
    {
        markerButtonPrefabClone = Instantiate(markerButtonPrefab, transform) as GameObject;
        markerButtonPrefabClone.transform.SetParent(transform, false);
        markerButtonPrefabClone.GetComponent<MarkerButton>().Setup(markerData);
    }

    private void removeAllButtons()
    {
        TransformEx.Clear(transform);
        
    }
}
