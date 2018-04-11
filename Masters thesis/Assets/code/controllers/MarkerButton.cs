using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerButton : MonoBehaviour {
    public Button button;
    public Button deleteMarker;
    public Text markerText;

    private MarkerData markerData;

	// Use this for initialization
	void Start () {
		
	}
	
    public void Setup(MarkerData marker)
    {

        markerText.text = marker.text;
    }
}
