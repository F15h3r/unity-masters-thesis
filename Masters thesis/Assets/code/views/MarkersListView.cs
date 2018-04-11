using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersListView : MonoBehaviour {
    public static bool isVisible = false;
    public GameObject MarkersMenuCanvas;

    void Start () {
        Debug.Log("start markersListView");
        MarkersMenuCanvas.SetActive(false);
	}

    public void toggleMarkersMenu()
    {
        Debug.Log("Toggle markersListView");
        isVisible = !isVisible;
        MarkersMenuCanvas.SetActive(isVisible);
        
    }
}
