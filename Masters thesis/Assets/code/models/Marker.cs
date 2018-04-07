using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.code
{
    class Marker : MonoBehaviour
    {
        public Vector3 worldCoords;
        public Vector3 markerPosition; // TODO: set private?
        private float distanceToUser = 0;
        public string text;
        
        public Marker(float lat, float lon)
        {
            worldCoords = new Vector3();
            worldCoords.y = float.MinValue;

            setLongitude(lon);
            setLatitude(lat);
        }

        public Marker(Vector3 worldCoords)
        {
            this.worldCoords = new Vector3();
            worldCoords.y = float.MinValue;

            setLongitude(worldCoords.x);
            setLatitude(worldCoords.z);
        }

        public Marker(float lat, float lon, float alt)
        {
            setLongitude(lon);
            setLatitude(lat);
            setAltitude(alt);
        }

        public void setLongitude(float lon)
        {
            if (lon < -180)
                worldCoords.x = lon + 360;
            else if (lon > 180)
                worldCoords.x = lon - 360;
            else
                worldCoords.x = lon;
        }

        public void setLatitude(float lat)
        {
            if (lat < -90)
            {
                worldCoords.z = -180 + System.Math.Abs(lat);
                setLongitude(worldCoords.x - 180);
            }
            else if (lat > 90)
            {
                worldCoords.z = 180 - System.Math.Abs(lat);
                setLongitude(worldCoords.x - 180);
            }
            else
                worldCoords.z = lat;
        }

        public bool setAltitude(float alt)
        {
            if(alt < 10000 && alt > -1000)
            {
                worldCoords.y = alt;
                return true;
            }
            Debug.LogError("Wrong altitude, value "+alt+" is outside -1000 and 10000!");
            return false;
        }

        private void setMarkerText(string content)
        {
            GetComponentInChildren<UnityEngine.UI.Text>().text = content;
        }

        public void setRelativeGamePosition()
        {
            markerPosition = new Vector3();
            if (GPSController.Instance.userLocationStable)
            {
                markerPosition.x = -(-GPSController.Instance.userWorldLocation.x + worldCoords.x) * MarkerController.markerScale.x;
                markerPosition.z = -(-GPSController.Instance.userWorldLocation.z + worldCoords.z) * MarkerController.markerScale.z;
                if (worldCoords.y == float.MinValue)
                    markerPosition.y = 0; // if no altitude information available, show marker at user altitude
                else
                    markerPosition.y = (-GPSController.Instance.userWorldLocation.y + worldCoords.y) * MarkerController.markerScale.y;
                
                setMarkerText(text + "\n" + getDistanceToUser());
                //Debug.Log("MARKER: " + text + " - worldLocation: " + worldCoords.ToString()+ " gamePosition: "
                //    + markerPosition.ToString() + " game distance to user:" + Vector3.Distance(markerPosition, new Vector3(0,0,0)));
            }
            else
                Debug.LogError("LONGITUDE/LATITUDE NOT SET, OR USER LOCATION UNKNOWN (YET?)");

            transform.position = markerPosition;
        }

        private void setDistanceToUser()
        {
            float ML = (worldCoords.z + GPSController.Instance.userWorldLocation.z) / 2;
            
            float KPD_lat = (float)(111.13209 - 0.56605 * Math.Cos(2 * ML) + 0.0012 * Math.Cos(4 * ML));
            float KPD_lon = (float)(111.41513 * Math.Cos(ML) - 0.09455 * Math.Cos(3 * ML) + 0.00012 * Math.Cos(5 * ML));
            float NS = KPD_lat * (worldCoords.z - GPSController.Instance.userWorldLocation.z);
            float EW = KPD_lon * (worldCoords.z - GPSController.Instance.userWorldLocation.z);
            Debug.Log("DIST = " + (float)Math.Sqrt(NS * NS + EW * EW) + "km");
            distanceToUser = (float)Math.Sqrt(NS * NS + EW * EW) * 1000;
        }

        public string getDistanceToUser()
        {
            setDistanceToUser();
            if (distanceToUser > 1000)
                return (distanceToUser / 1000).ToString("0.0") + "km";
            if (distanceToUser < 1000)
                return ((Math.Round(distanceToUser/100, 0)*100)).ToString() + "m";
            else
                return distanceToUser.ToString("0") + "m";
        }
    }
}
