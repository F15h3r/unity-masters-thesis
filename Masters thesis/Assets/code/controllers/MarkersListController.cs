using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersListController : MonoBehaviour {
    public static MarkersListController Instance { get; set; }
    public GameObject markerButtonInstanceParent;
    public GameObject markerButtonPrefab;
    private GameObject markerButtonPrefabClone;

    public bool isDisplayed = false;
    public GameObject MarkersMenuCanvas;

    // Use this for initialization
    void Start () {
        Instance = this;
        MarkersMenuCanvas.SetActive(isDisplayed);
    }

    public void toggleMenuBtnClick()
    {
        toggleMarkersMenu();
    }


    public void toggleMarkersMenu(bool webMarkers = false, List<MarkerData> markersList = null)
    {
        MarkerInfoPopUpController.Instance.closeMarkerInfoPopup();
        MarkerAddPopupController.Instance.closeMarkerAddPopup();

        isDisplayed = !isDisplayed;
        
        if (isDisplayed)
            refreshAllMarkersList(webMarkers, markersList);
            
        MarkersMenuCanvas.SetActive(isDisplayed);
    }

    public void closeMarkersMenu()
    {
        if (isDisplayed)
        {
            isDisplayed = false;
            MarkersMenuCanvas.SetActive(false);
        }
    }


    public void refreshAllMarkersList(bool webMarkers = false, List<MarkerData> markersList = null)
    {
        if(!webMarkers)
        {
            removeAllMarkersMenuItems();
            addAllLocalMarkersMenuItems();
        }
        else
        {
            removeAllMarkersMenuItems();
            if (markersList != null)
                addAllMarkersFromList(markersList);
            else
                Debug.LogError("No Markers provided to display!");
        }
            
    }


    private void addAllLocalMarkersMenuItems()
    {
        foreach(GameObject marker in MarkerController.Instance.markers)
            addMarkerButtonToList(marker.GetComponent<Marker>().data);
    }

    public void addAllMarkersFromList(List<MarkerData> markers)
    {

        foreach (MarkerData marker in markers)
            addMarkerButtonToList(marker);
    }

    private void addMarkerButtonToList(MarkerData markerData)
    {
        markerButtonPrefabClone = Instantiate(markerButtonPrefab, markerButtonInstanceParent.transform) as GameObject;
        markerButtonPrefabClone.transform.SetParent(markerButtonInstanceParent.transform, false);
        markerButtonPrefabClone.GetComponent<AllMarkersListItem>().Setup(markerData);
    }

    private void removeAllMarkersMenuItems()
    {
        for(int i = markerButtonInstanceParent.transform.childCount - 1; i >= 0; i--)
            Destroy(markerButtonInstanceParent.transform.GetChild(i).gameObject);
    }


}
