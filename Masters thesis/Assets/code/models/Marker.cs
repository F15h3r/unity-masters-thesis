﻿using Assets.code.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.code
{
    class Marker : MonoBehaviour
    {
        public MarkerData data;

        public void Setup(Vector3 worldCoords, string name, string description)
        {
            print("Marker  setup for " + name);
            data = new MarkerData();
            setLatitude(worldCoords.z);
            setLongitude(worldCoords.x);
            if (worldCoords.y != float.MinValue)
                setAltitude(worldCoords.y);
            setMarkerName(name);
            data.description = description;
            data.dateAdded = DateTime.Now.ToString("dd.MM.yyyy, H:mm");

            transform.gameObject.GetComponentInChildren<Button>().onClick.AddListener(
                delegate { MarkerInfoPopUpController.Instance.showMarkerInfoPopup(data); });
            transform.Rotate(new Vector3(0, 180, 0)); // Rotate the prefab
            transform.LookAt(new Vector3(0, -10, 0)); // Look at the camera

            setRelativeGamePosition();
            print("Marker " + name + " set up correctly");
        }

        public void loadFromMarkerData(MarkerData md)
        {
            data = md;
            setRelativeGamePosition();
            transform.gameObject.GetComponentInChildren<Button>().onClick.AddListener(
                delegate { MarkerInfoPopUpController.Instance.showMarkerInfoPopup(data); });
            transform.Rotate(new Vector3(0, 180, 0)); // Rotate the prefab
            transform.LookAt(new Vector3(0, -20, 0)); // Look at the camera
        }

        public void setLongitude(float lon)
        {
            if (lon < -180)
                data.worldCoords.x = lon + 360;
            else if (lon > 180)
                data.worldCoords.x = lon - 360;
            else
                data.worldCoords.x = lon;
        }

        public float getLongitude()
        {
            return data.worldCoords.x;
        }

        public void setLatitude(float lat)
        {
            if (lat < -90)
            {
                data.worldCoords.z = -180 + System.Math.Abs(lat);
                setLongitude(data.worldCoords.x - 180);
            }
            else if (lat > 90)
            {
                data.worldCoords.z = 180 - System.Math.Abs(lat);
                setLongitude(data.worldCoords.x - 180);
            }
            else
                data.worldCoords.z = lat;
        }

        public float getLatitude()
        {
            return data.worldCoords.z;
        }

        public bool setAltitude(float alt)
        {
            if(alt < 10000 && alt > -1000)
            {
                data.worldCoords.y = alt;
                return true;
            }
            Debug.LogError("Wrong altitude, value "+alt+" is outside -1000 and 10000!");
            return false;
        }

        public int getAltitude()
        {
            return (int)data.worldCoords.y;
        }

        public void setMarkerName(string content)
        {
            data.name = content;
        }


        public string getMarkerName()
        {
            return data.name;
        }

        public void setMarkerDisplayText()
        {
            GetComponentInChildren<UnityEngine.UI.Text>().text = data.name + "\n"
                + data.getDistanceToUser();// + "\n" + Vector3.Distance(data.markerPosition, Application.userGamePosition);
        }

        public void setRelativeGamePosition()
        {
            data.markerPosition = new Vector3();
            if (GPSController.Instance.userLocationStable)
            {
                data.markerPosition.x = -(-GPSController.Instance.userWorldLocation.x + data.worldCoords.x) * MarkerController.markerScale.x;
                data.markerPosition.z = -(-GPSController.Instance.userWorldLocation.z + data.worldCoords.z) * MarkerController.markerScale.z;
                if (data.worldCoords.y == float.MinValue)
                    data.markerPosition.y = 0; // if no altitude information available, show marker at user altitude (0 on Y axis)
                else
                    data.markerPosition.y = (-GPSController.Instance.userWorldLocation.y + data.worldCoords.y) * MarkerController.markerScale.y;

                setMarkerDisplayText();
                //Debug.Log("MARKER: " + text + " - worldLocation: " + worldCoords.ToString()+ " gamePosition: "
                //    + markerPosition.ToString() + " game distance to user:" + Vector3.Distance(markerPosition, new Vector3(0,0,0)));
            }
            else
                Debug.LogError("LONGITUDE/LATITUDE NOT SET, OR USER LOCATION UNKNOWN (YET?)");

            transform.position = data.markerPosition;
            //float scaleFactor = Vector3.Distance(data.markerPosition, Application.userGamePosition) / 200;
            //if(Vector3.Distance(data.markerPosition, Application.userGamePosition) < 100)
            //    transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        public MarkerData getMarkerData()
        {
            return data;
        }
    }
}