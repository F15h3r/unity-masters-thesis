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

	void Awake () {
        Instance = this;
        popUp.SetActive(isDisplayed);
	}

    public void showMarkerInfoPopup(MarkerData markerData)
    {
        //MarkersListController.Instance.closeMarkersMenu();
        MarkerAddPopupController.Instance.closeMarkerAddPopup();
        
        md = markerData;
        popUp.GetComponentInChildren<Text>().text = md.name;
        popUp.transform.FindChild("markerLatLon").GetComponent<Text>().text
            = "Lat: " + md.worldCoords.z + ", Lon: " + md.worldCoords.x + ", Alt: " + md.worldCoords.y;
        popUp.transform.FindChild("markerDate").GetComponent<Text>().text = markerData.dateAdded;
        popupMarkerInfoText.GetComponent<Text>().text = md.description;
        popUp.transform.FindChild("closeButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.closeMarkerInfoPopup);
        
        popUp.transform.FindChild("hideButton").GetComponent<Button>().onClick.AddListener(MarkerInfoPopUpController.Instance.toggleMarkerVisibility);

        if (md.acquiredOnline)
        {
            popUp.transform.FindChild("hideButton").gameObject.SetActive(false);
        }
        else
        {
            popUp.transform.FindChild("hideButton").gameObject.SetActive(true);
        }


        isDisplayed = true;
        popUp.SetActive(isDisplayed);
    }

    public void closeMarkerInfoPopup()
    {
        isDisplayed = false;
        popUp.SetActive(isDisplayed);
        
    }

    public void toggleMarkerVisibility()
    {
            MarkerController.Instance.toggle3DMarkerVisibility(md);
    }
}
