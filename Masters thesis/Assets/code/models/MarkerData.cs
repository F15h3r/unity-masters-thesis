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
        public static double world_R = 6371000; // world r in meters

        public double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private void setDistanceToUser()
        {
            double worldCoordsLat_rad = ToRadians(worldCoords.z);
            double userCoordsLat_rad = ToRadians(GPSController.Instance.userWorldLocation.z);
            double deltaLon = ToRadians(GPSController.Instance.userWorldLocation.x - worldCoords.x);
            
            distanceToUser = (float) (Math.Acos(Math.Sin(worldCoordsLat_rad) * Math.Sin(userCoordsLat_rad)
                + Math.Cos(worldCoordsLat_rad) * Math.Cos(userCoordsLat_rad) * Math.Cos(deltaLon)) * world_R);
        }

        /*
         * Return user readable distance to marker
         */
        public string getDistanceToUser()
        {
            setDistanceToUser();

            if (distanceToUser > 1000)
                return (distanceToUser / 1000).ToString("0.0") + "km";
            if (distanceToUser < 1000 && distanceToUser >= 100)
                return ((Math.Round(distanceToUser / 10, 0) * 10)).ToString() + "m";
            if (distanceToUser < 100)
                return ((Math.Round(distanceToUser, 0))).ToString() + "m";
            else
                return distanceToUser.ToString("0") + "m";
        }

    }
}
