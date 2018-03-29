using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {

    private void Awake()
    {
        RESTServicesManager.Instance = gameObject.AddComponent(typeof(RESTServicesManager)) as RESTServicesManager;
        //MarkerManager.Instance = gameObject.AddComponent(typeof(MarkerManager)) as MarkerManager;        
    }

}
