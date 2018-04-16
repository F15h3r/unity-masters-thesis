using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersListView : MonoBehaviour {
    public static bool isVisible = false;
    public GameObject MarkersMenuCanvas;

    void Start () {
        MarkersMenuCanvas.SetActive(false);
	}

    public void toggleMarkersMenu()
    {
        isVisible = !isVisible;
        MarkersMenuCanvas.SetActive(isVisible);
    }
}
