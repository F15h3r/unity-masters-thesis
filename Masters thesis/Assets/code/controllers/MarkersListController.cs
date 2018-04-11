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
        removeButtons();
        addAllButtons();
    }

    private void addMarkerButtonToList(MarkerData markerData)
    {
        markerButtonPrefabClone = Instantiate(markerButtonPrefab, transform) as GameObject;
        if (markerButtonPrefabClone != null)
            markerButtonPrefabClone.GetComponent<MarkerButton>().Setup(markerData);
        else
            Debug.LogError("markerButtonPrefabClone = null!");
    }

    private void addAllButtons()
    {
        foreach(GameObject marker in MarkerController.Instance.markers)
            addMarkerButtonToList(marker.GetComponent<Assets.code.Marker>().getMarkerData());
    }

    private void removeButtons()
    {

        while(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        
    }

    private void removeMarker(MarkerData marker) // TODO: MOGOČE PREMAKNI TOLE V MARKERcONTROLLER ??? iN OD TAM KLIČI SAMO REFRESHDISPLAY???
    {
        for(int i = MarkerController.Instance.markers.Count-1; i>=0;i--)
        {
            if(MarkerController.Instance.markers[i].GetComponent<MarkerData>().Equals(marker))
            {
                MarkerController.Instance.markers.RemoveAt(i);
                refreshDisplay();
                break;
            }
        }
    }
}
