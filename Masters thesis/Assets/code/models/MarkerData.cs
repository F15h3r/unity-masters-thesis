using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.code.models
{
    [System.Serializable]
    public class MarkerData
    {
        public Vector3 worldCoords;
        public Vector3 markerPosition;
        public float distanceToUser = 0;
        public string name;
        public string description;
        public string dateAdded;
        public bool visible = true;


        private void setDistanceToUser()
        {
            float ML = (worldCoords.z + GPSController.Instance.userWorldLocation.z) / 2;

            float KPD_lat = (float)(111.13209 - 0.56605 * Math.Cos(2 * ML) + 0.0012 * Math.Cos(4 * ML));
            float KPD_lon = (float)(111.41513 * Math.Cos(ML) - 0.09455 * Math.Cos(3 * ML) + 0.00012 * Math.Cos(5 * ML));
            float NS = KPD_lat * (worldCoords.z - GPSController.Instance.userWorldLocation.z);
            float EW = KPD_lon * (worldCoords.z - GPSController.Instance.userWorldLocation.z);
            distanceToUser = (float)Math.Sqrt(NS * NS + EW * EW) * 1000;
        }

        public string getDistanceToUser()
        {
            setDistanceToUser();
            if (distanceToUser > 1000)
                return (distanceToUser / 1000).ToString("0.0") + "km";
            if (distanceToUser < 1000)
                return ((Math.Round(distanceToUser / 100, 0) * 100)).ToString() + "m";
            if (distanceToUser < 100)
                return ((Math.Round(distanceToUser / 10, 0) * 100)).ToString() + "m";
            else
                return distanceToUser.ToString("0") + "m";
        }
    }
}
