using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMarkersListItem : MonoBehaviour {
    public Button deleteButton;
    public Button toggleVisible;
    public Button markerButton;
    public Text markerText;
    public Text markerDistanceText;
    public Sprite addButtonSprite;
    public AllMarkersListItem clickListenerScript;


    private MarkerData markerData;
	
    public void Setup(MarkerData md)
    {
        markerData = md;

        markerText.text = markerData.name;
        markerDistanceText.text = markerData.getDistanceToUserString();

        if(md.acquiredOnline)
        {
            toggleVisible.gameObject.SetActive(false);
            deleteButton.transform.GetComponent<Image>().sprite = addButtonSprite;
        }
        else
        {
            if (md.visible)
                toggleVisible.gameObject.GetComponent<Text>().text = "Hide";
            else
                toggleVisible.gameObject.GetComponent<Text>().text = "Show";
        }


        if(markerData.acquiredOnline)
            deleteButton.onClick.AddListener(saveMarker);
        else
            deleteButton.onClick.AddListener(removeClick);

        toggleVisible.onClick.AddListener(toggleVisibility);
        markerButton.onClick.AddListener(selectMarker);
    }

    public void removeClick()
    {
        MarkerController.Instance.remove3DMarkerInstance(markerData);
        MarkersListController.Instance.refreshAllMarkersList();
        GoogleMapsController.Instance.reloadMapImage();
    }

    public void saveMarker()
    {
        MarkerController.Instance.add3DMarkerInstance(markerData.worldCoords, markerData.name, markerData.description);
        MarkersListController.Instance.refreshAllMarkersList();
        GoogleMapsController.Instance.reloadMapImage();
    }

    private void toggleVisibility()
    {

        if(!markerData.visible)
            toggleVisible.gameObject.GetComponent<Text>().text = "Hide";
        else
            toggleVisible.gameObject.GetComponent<Text>().text = "Show";

        MarkerController.Instance.toggle3DMarkerVisibility(markerData);
        MarkersListController.Instance.refreshAllMarkersList();
        GoogleMapsController.Instance.reloadMapImage();
    }

    public void selectMarker()
    {
        MarkerInfoPopUpController.Instance.showMarkerInfoPopup(markerData);
    }
}
