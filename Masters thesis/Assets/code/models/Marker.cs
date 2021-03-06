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
            data = new MarkerData();
            setLatitude(worldCoords.z);
            setLongitude(worldCoords.x);
            if (worldCoords.y != float.MinValue)
                setAltitude(worldCoords.y);
            setMarkerName(name);
            data.description = description;
            data.dateAdded = Application.Utils.dateToday();

            transform.gameObject.GetComponentInChildren<Button>().onClick.AddListener(
                delegate { MarkerInfoPopUpController.Instance.showMarkerInfoPopup(data); });
            transform.Rotate(new Vector3(0, 180, 0)); // Rotate the prefab
            transform.LookAt(new Vector3(0, -10, 0)); // Look at the camera

            setRelativeGamePosition();
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
                + data.getDistanceToUserString();
        }

        public void setRelativeGamePosition()
        {
            data.markerPosition = new Vector3();

            if (GPSController.Instance.userLocationStable)
            {
                data.markerPosition.x = -(-GPSController.Instance.userWorldLocation.x + data.worldCoords.x);
                data.markerPosition.z = -(-GPSController.Instance.userWorldLocation.z + data.worldCoords.z);

                data.markerPosition.Normalize();

                data.markerPosition.Scale(new Vector3(
                    data.getDistanceToUserFloat() * MarkerController.Instance.unityWorldRatio.x,
                    1,
                    data.getDistanceToUserFloat() * MarkerController.Instance.unityWorldRatio.z)); // unity : world ratio in UnityWorldRatio

                // set up markers altitude we otherwise loose with normalization
                if (data.worldCoords.y == float.MinValue)
                    data.markerPosition.y = 0; // if no altitude information available, show marker at user altitude (0 on Y axis)
                else
                    data.markerPosition.y = (-GPSController.Instance.userWorldLocation.y + data.worldCoords.y) 
                        * MarkerController.Instance.unityWorldRatio.y;


                setMarkerDisplayText();
                
                //Debug.Log("MARKER: " + text + " - worldLocation: " + worldCoords.ToString()+ " gamePosition: "
                //    + markerPosition.ToString() + " game distance to user:" + Vector3.Distance(markerPosition, new Vector3(0,0,0)));
            }
            else
                Debug.LogError("LONGITUDE/LATITUDE NOT SET, OR USER LOCATION UNKNOWN (YET?)");

            transform.position = data.markerPosition;
        }

        public MarkerData getMarkerData()
        {
            return data;
        }
    }
}