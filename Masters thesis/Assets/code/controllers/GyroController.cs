using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour {
    public static GyroController Instance { get; set; }
    public Gyroscope gyroscope;
    public bool gyroAvailable = false;
    private Quaternion rotation;
    public Quaternion deviceOrientation;

    private void Awake()
    {
        Instance = this;
        rotation = new Quaternion(0, 0, 1, 0);
        gyroscope = Input.gyro;

        if(gyroscope != null)
        {
            gyroscope.enabled = true;
            gyroAvailable = true;
        }        
    }

    void Start () {
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.LogError("NO GYROSCOPE FOUND");
            return;
        }
    }
	
	void Update () {
        if (gyroAvailable)
        {
            deviceOrientation = gyroscope.attitude * rotation;
        }
    }
}
