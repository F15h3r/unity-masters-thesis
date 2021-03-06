﻿using Assets.code.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.code.controllers
{
    class InternalDataController
    {
        public static string
            markersData = "markersData",
            developmentSetting = "devON",
            GPSRefreshFreqSetting = "gpsRefFreq",
            mapsRefreshFreqSetting = "mapsRefFreq",
            mapsZoomLevelSetting = "mapsZoomLevel";


        public static void init()
        {
            if (!PlayerPrefs.HasKey(markersData))
                PlayerPrefs.SetString(markersData, "");

            if (!PlayerPrefs.HasKey(developmentSetting))
                PlayerPrefs.SetString(developmentSetting, "False");

            if (!PlayerPrefs.HasKey(GPSRefreshFreqSetting))
                PlayerPrefs.SetString(GPSRefreshFreqSetting, "5");

            if (!PlayerPrefs.HasKey(mapsRefreshFreqSetting))
                PlayerPrefs.SetString(mapsRefreshFreqSetting, "0");

            if (!PlayerPrefs.HasKey(mapsZoomLevelSetting))
                PlayerPrefs.SetString(mapsZoomLevelSetting, "14");

            PlayerPrefs.Save();
        }

        public static void readMarkersFromMemory()
        {
            if (PlayerPrefs.HasKey(markersData))
            {
                string jsonMarkersData = PlayerPrefs.GetString(markersData);
                MarkersDataList mdl = new MarkersDataList();

                try
                {
                    JsonUtility.FromJsonOverwrite(jsonMarkersData, mdl);

                    foreach (MarkerData md in mdl.markersDataList)
                    {
                        MarkerController.Instance.load3DMarkerInstance(md);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Deserialization error: " + e.ToString());
                }

            }
            else
            {
                Debug.LogError("Markers data playerPrefs doesn't exist!");
            }
        }

        private static string serializeMarkers()
        {
            string dataToSave = "{\"markersDataList\":[";
            if (MarkerController.Instance.markers.Count > 0)
            {
                foreach (GameObject go in MarkerController.Instance.markers)
                {
                    dataToSave += JsonUtility.ToJson(go.GetComponent<Marker>().data) + ",";
                }
                dataToSave = dataToSave.Substring(0, dataToSave.Length - 1) + "]}";
            }
            else
            {
                dataToSave += "]}";
            }

            return dataToSave;
        }

        public static void saveSerializedMarkersList()
        {
            PlayerPrefs.SetString(markersData, serializeMarkers());
            PlayerPrefs.Save();
        }

        public static void saveValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static string loadValue(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }

            Debug.LogError("Can't load value from key: " + key);
            return "";
        }
    }
}
