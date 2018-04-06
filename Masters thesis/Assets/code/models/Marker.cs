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

        public string text
        {
            set
            {
                GetComponentInChildren<UnityEngine.UI.Text>().text = value;
            }
            get
            {
                return GetComponentInChildren<UnityEngine.UI.Text>().text;
            }
        }

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

        public void setRelativeGamePosition()
        {
            markerPosition = new Vector3();
            if (worldCoords != null && GPSController.Instance.userLocationStable)
            {
                //Debug.Log("USER LOCATION ON MARKER UPDATE: " + GPSController.Instance.userWorldLocation.ToString());

                markerPosition.x = -(-GPSController.Instance.userWorldLocation.x + worldCoords.x) * MarkerController.markerScale.x;
                markerPosition.z = -(-GPSController.Instance.userWorldLocation.z + worldCoords.z) * MarkerController.markerScale.z;
                if (worldCoords.y == float.MinValue)
                    markerPosition.y = 0; // if no altitude information available, show marker at user altitude
                else
                    markerPosition.y = (-GPSController.Instance.userWorldLocation.y + worldCoords.y) * MarkerController.markerScale.y;

                //Debug.Log("MARKER worldLocation: " + worldPosition.ToString()+ " gamePosition: " + markerPosition.ToString() + " text: " + text);
            }
            else
                Debug.LogError("LONGITUDE/LATITUDE NOT SET, OR USER LOCATION UNKNOWN (YET?)");

            transform.position = markerPosition;
        }
    }
}
