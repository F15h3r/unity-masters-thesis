using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //RESTServiceController.Instance = gameObject.AddComponent(typeof(RESTServiceController)) as RESTServiceController;
        //MarkerManager.Instance = gameObject.AddComponent(typeof(MarkerManager)) as MarkerManager;        
    }

}
