using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMarkersListItem : MonoBehaviour {
    public Button deleteButton;
    public Button toggleVisible;
    public Text markerText;
    public AllMarkersListItem clickListenerScript;


    private MarkerData markerData;

	// Use this for initialization
	void Start () {
        
    }
	
    public void Setup(MarkerData md)
    {
        markerData = md;

        markerText.text = markerData.name;
        if (md.visible)
            toggleVisible.gameObject.GetComponent<Text>().text = "Hide";
        else
            toggleVisible.gameObject.GetComponent<Text>().text = "Show";

        deleteButton.onClick.AddListener(removeClick);
        toggleVisible.onClick.AddListener(toggleVisibility);
    }

    public void removeClick()
    {
        MarkerController.Instance.remove3DMarkerInstance(markerData);
        MarkersListController.Instance.refreshAllMarkersList();
        GoogleMapsController.Instance.reloadMapImage();
    }

    private void toggleVisibility()
    {
        markerData.visible = !markerData.visible;

        if(markerData.visible)
        {
            toggleVisible.gameObject.GetComponent<Text>().text = "Hide";
        }
        else
        {
            toggleVisible.gameObject.GetComponent<Text>().text = "Show";
        }

        MarkerController.Instance.update3DMarkerVisibility(markerData);

        gameObject.GetComponentInParent<MarkersListController>().refreshAllMarkersList();
        GoogleMapsController.Instance.reloadMapImage();
    }
}
