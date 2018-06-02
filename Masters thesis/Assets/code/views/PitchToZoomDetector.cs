using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchToZoomDetector : MonoBehaviour {
    public static PitchToZoomDetector Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag)*100;

            foreach(GameObject marker in MarkerController.Instance.markers)
            {
                marker.transform.localScale = marker.transform.localScale * (1-deltaMagnitudeDiff/10000);
            }
        }
    }
}
