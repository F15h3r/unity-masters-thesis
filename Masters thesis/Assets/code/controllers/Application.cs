using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Application : MonoBehaviour {
    public static Vector3 userGamePosition;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        userGamePosition = new Vector3(0, 0, 0);     
    }
}

/*
 * Source: Answer by yatagarasu · Jan 08, 2014 at 03:46 PM
 * https://answers.unity.com/questions/611850/destroy-all-children-of-object.html
 */
public static class TransformEx
{
    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
}


