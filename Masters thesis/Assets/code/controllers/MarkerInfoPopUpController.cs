using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerInfoPopUpController : MonoBehaviour {
    public static MarkerInfoPopUpController Instance { set; get; }
    public GameObject popUp;
    private MarkerData md;

	// Use this for initialization
	void Start () {
        Instance = this;
        popUp.SetActive(false);
	}

    public void showMarkerInfoPopup(MarkerData markerData)
    {
        Debug.Log("Popup show called"); // TODO: Preveri vse to..
        md = markerData;
        popUp.GetComponentInChildren<Text>().text = md.name;
        popUp.transform.FindChild("markerLatLon").GetComponent<Text>().text
            = "Lat: " + md.worldCoords.z + ", Lon: " + md.worldCoords.x;
        popUp.transform.FindChild("markerInfo").GetComponent<Text>().text = md.description;
        popUp.transform.FindChild("closeButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.closeMarkerInfoPopup);
        popUp.transform.FindChild("removeButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.removeMarkerInstance);
        
    }

    public void closeMarkerInfoPopup()
    {
        Debug.Log("Popup close called");
        popUp.SetActive(false);
    }

    public void removeMarkerInstance()
    {
        Debug.Log("Popup remove marker called");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
