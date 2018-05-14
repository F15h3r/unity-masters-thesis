using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersListController : MonoBehaviour {
    public static MarkersListController Instance { set; get; }
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


    public void toggleMarkersMenu()
    {
        MarkerInfoPopUpController.Instance.closeMarkerInfoPopup();

        isDisplayed = !isDisplayed;

        if (isDisplayed)
            refreshAllMarkersList();
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


    public void refreshAllMarkersList()
    {
        removeAllMarkersMenuItems();
        addAllMarkersMenuItems();
    }


    private void addAllMarkersMenuItems()
    {
        foreach(GameObject marker in MarkerController.Instance.markers)
            addMarkerButtonToList(marker.GetComponent<Marker>().data);
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
