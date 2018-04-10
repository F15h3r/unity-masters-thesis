using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashGPSLocationWarning: MonoBehaviour {
    public float duration = 0.5f;
    private float currentDuration = 0;
    private RawImage img;

	// Use this for initialization
	void Start () {
        img = GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GPSController.Instance.userLocationStable)
        {
            currentDuration += Time.unscaledDeltaTime;
            if (currentDuration >= duration)
            {
                currentDuration = 0;
                img.enabled = !img.enabled;
            }
        }
        else
            img.enabled = false;

    }
}
