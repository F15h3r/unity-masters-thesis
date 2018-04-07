using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Application : MonoBehaviour {
    public static Vector3 userGamePosition;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        userGamePosition = new Vector3(0, 0, 0);
        //RESTServiceController.Instance = gameObject.AddComponent(typeof(RESTServiceController)) as RESTServiceController;
        //MarkerManager.Instance = gameObject.AddComponent(typeof(MarkerManager)) as MarkerManager;        
    }

}
