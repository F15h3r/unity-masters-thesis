using Assets.code.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Application : MonoBehaviour {
    public static Vector3 userGamePosition;
    public static string googleApiKey = "AIzaSyApdbgfaSwua1iDr0dsAl3v4I6ZJ4Emc9c";

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        userGamePosition = new Vector3(0, 0, 0);     
    }



    public static class Utils
    {
        internal static string dateToday()
        {
            return DateTime.Now.ToString("dd.MM.yyyy, H:mm");
        }
    }
}


