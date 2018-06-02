using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerInfoPopUpController : MonoBehaviour {
    public static MarkerInfoPopUpController Instance { set; get; }
    public GameObject popUp;
    public GameObject popupMarkerInfoText;
    private MarkerData md;
    public bool isDisplayed = false;

	// Use this for initialization
	void Start () {
        Instance = this;
        popUp.SetActive(isDisplayed);
	}

    public void showMarkerInfoPopup(MarkerData markerData)
    {
        MarkersListController.Instance.closeMarkersMenu();
        MarkerAddPopupController.Instance.closeMarkerAddPopup();
        
        md = markerData;
        popUp.GetComponentInChildren<Text>().text = md.name;
        popUp.transform.FindChild("markerLatLon").GetComponent<Text>().text
            = "Lat: " + md.worldCoords.z + ", Lon: " + md.worldCoords.x + ", Alt: " + md.worldCoords.y;
        popUp.transform.FindChild("markerDate").GetComponent<Text>().text = markerData.dateAdded;
        popupMarkerInfoText.GetComponent<Text>().text = md.description;
        popUp.transform.FindChild("closeButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.closeMarkerInfoPopup);
        popUp.transform.FindChild("removeButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.removeMarkerInstance);

        popUp.SetActive(true);
    }

    public void closeMarkerInfoPopup()
    {
        isDisplayed = false;
        popUp.SetActive(isDisplayed);
    }

    public void removeMarkerInstance()
    {
        MarkerController.Instance.remove3DMarkerInstance(md);
        popUp.SetActive(false);
    }
}
