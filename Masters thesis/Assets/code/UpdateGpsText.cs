using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGpsText : MonoBehaviour {
    public Text textToDisplay;
    private int frames, FPS;
    private float FPSTimer;

    private void Start()
    {
        FPSTimer = 1f;
    }

    private void Update ()
    {
        FPSUpdate();

        textToDisplay.text = "LAT: " + GPS.Instance.latitude + " : " + GPS.Instance.z + "\n"
            + "LON: " + GPS.Instance.longitude + " : " + GPS.Instance.x + "\n"
            + "ALT: " + GPS.Instance.altitude + " : " + GPS.Instance.y
            + "\n" + FPS + " FPS";


        
        /*
        if (markers[0].getAltitude() != float.MinValue)
            Debug.Log("MARKER LON:" + markers[0].getLongitude().ToString() +
                " LAT:" + markers[0].getLatitude().ToString() +
                " ALT:" + markers[0].getAltitude().ToString());
        
        /*
        // premik objekta glede na svetovne koordinate
        Vector3 mv = new Vector3(0, 1, 0) * Time.deltaTime;

        Camera c = gameObject.GetComponent(typeof(Camera)) as Camera;
        if (c != null)
            c.transform.Translate(Vector3.up * Time.deltaTime * 50, Space.World);
            
        else
            Debug.Log("NI KAMERE");
        */
    }

    private void FPSUpdate()
    {
        frames++;
        FPSTimer -= Time.unscaledDeltaTime;
        if (FPSTimer <= 0f)
        {
            FPS = frames;
            FPSTimer = 1f;
            frames = 0;
        }
    }
}
