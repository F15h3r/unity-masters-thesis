using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserIconOrientController : MonoBehaviour {
    public static UserIconOrientController Instance { get; set; }
    public GameObject userIcon;

	// Use this for initialization
	void Awake () {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        userIcon.transform.localRotation = GyroController.Instance.deviceOrientation;
        userIcon.transform.Rotate(0, 0, 180); // additional orientation - Quaternions point to south..
	}
}
