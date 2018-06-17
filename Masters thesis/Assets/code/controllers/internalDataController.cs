using Assets.code.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.code.controllers
{
    class InternalDataController
    {
        private const string markersDataPlayerPrefs = "markersData";

        public static void readMarkersFromMemory()
        {
            if (PlayerPrefs.HasKey(markersDataPlayerPrefs))
            {
                string jsonMarkersData = PlayerPrefs.GetString(markersDataPlayerPrefs);
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
            PlayerPrefs.SetString(markersDataPlayerPrefs, serializeMarkers());
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
