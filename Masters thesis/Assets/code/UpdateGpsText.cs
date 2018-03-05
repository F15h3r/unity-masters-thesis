using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGpsText : MonoBehaviour {
    public Text textToDisplay;
    
	private void Update ()
    {
        textToDisplay.text = "LAT: " + GPS.Instance.latitude + "\n"
            + "LON: " + GPS.Instance.longitude + "\n"
            + "ALT: " + GPS.Instance.altitude;
	}
}
