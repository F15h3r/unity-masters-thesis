using Assets.code;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerButton : MonoBehaviour {
    public Button deleteButton;
    public Button toggleVisible;
    public Text markerText;
    public MarkerButton clickListenerScript;


    private MarkerData markerData;

	// Use this for initialization
	void Start () {
        
    }
	
    public void Setup(MarkerData md)
    {
        markerData = md;

        markerText.text = markerData.text;
        deleteButton.onClick.AddListener(removeClick);
    }

    public void removeClick()
    {
        MarkerController.Instance.remove3DMarkerInstance(markerData);
        Debug.Log("removed 3D marker prefab GameObject instance");
        gameObject.GetComponentInParent<MarkersListController>().refreshDisplay();
    }

    private void toggleVisibility()
    {
        Debug.Log("Hide click listener called");
        if(markerData.visible)
        {
            MarkerController.Instance.hideMarker(markerData);
            gameObject.GetComponentInParent<MarkersListController>().refreshDisplay();

        } // TODO: ENAKO ZA PRIKAZ...


    }
}
