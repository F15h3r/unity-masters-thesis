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
        public float distanceFromUser = 0;
        public string name;
        public string description;
        public string dateAdded;
        public bool visible = true;
        public bool acquiredOnline = false;
        public static double world_R = 6371000; // world r in meters

        public double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /*
         *  Distance calculation
         */
        private void calculateDistanceFromUser()
        {
            double worldCoordsLat_rad = ToRadians(worldCoords.z);
            double userCoordsLat_rad = ToRadians(GPSController.Instance.userWorldLocation.z);
            double deltaLon = ToRadians(GPSController.Instance.userWorldLocation.x - worldCoords.x);
            
            distanceFromUser = (float) (Math.Acos(Math.Sin(worldCoordsLat_rad) * Math.Sin(userCoordsLat_rad)
                + Math.Cos(worldCoordsLat_rad) * Math.Cos(userCoordsLat_rad) * Math.Cos(deltaLon)) * world_R);
        }

        /*
         * Return user readable distance to marker
         */
        public string getDistanceToUserString()
        {
            calculateDistanceFromUser();

            if (distanceFromUser > 1000)
                return (distanceFromUser / 1000).ToString("0.0") + "km";
            if (distanceFromUser < 1000 && distanceFromUser >= 100)
                return ((Math.Round(distanceFromUser / 10, 0) * 10)).ToString() + "m";
            if (distanceFromUser < 100)
                return ((Math.Round(distanceFromUser, 0))).ToString() + "m";
            else
                return distanceFromUser.ToString("0") + "m";
        }

        public float getDistanceToUserFloat()
        {
            calculateDistanceFromUser();

            return distanceFromUser;
        }

    }
}
