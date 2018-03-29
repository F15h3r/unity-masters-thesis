using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.code
{
    class Marker : MonoBehaviour
    {
        private float longitude;
        private float latitude;
        private float altitude;
        public float markerX, markerY, markerZ;

        private string tag { get; set; }

        public Marker()
        {
            longitude = latitude = altitude = markerX = markerY = float.MinValue;
        }

        public Marker(float lon, float lat, bool altitudeFromGoogle = true)
        {
            longitude = lon;
            latitude = lat;
            this.tag = tag;
            if (altitudeFromGoogle)
                setAltitudeFromRESTService();
        }

        public Marker(float lon, float lat, float alt)
        {
            longitude = lon;
            altitude = alt;
            latitude = lat;
            this.tag = tag;
        }

        public void setLongitude(float lon)
        {
            if (lon < -180)
                longitude = lon + 360;
            else if (lon > 180)
                longitude = lon - 360;
            else
                longitude = lon;
        }

        public void setLatitude(float lat)
        {
            if(lat < 90)
            {
                latitude = -180 + Math.Abs(lat);
                setLongitude(longitude - 180);
            } else if(lat > 90)
            {
                latitude = 180 - Math.Abs(lat);
                setLongitude(longitude - 180);
            }
        }


        public float getLatitude()
        {
            return latitude;
        }

        public float getLongitude()
        {
            return longitude;
        }

        public float getAltitude()
        {
            return altitude;
        }

        public void setAltitudeFromRESTService()
        {
            if (latitude != float.MinValue && longitude != float.MinValue)
            {
                altitude = RESTServicesManager.Instance.getElevation(longitude, latitude);
            }
            else
                Debug.Log("LONGITUDE OR LATITUDE NOT SET! CHECK: " + ToString());
        }

        public void translateRelativeToUser()
        {
            if (latitude != float.MinValue && longitude != float.MinValue)
            {
                markerX = -GPS.Instance.latitude + latitude * 10;
                markerY = -GPS.Instance.longitude + longitude * 10;
                markerZ = -GPS.Instance.altitude + altitude * 10;
                Debug.Log("MARKER: " + markerX + " " + markerY + " " + markerZ);
            }
            else
                Debug.Log("LONGITUDE OR LATITUDE NOT SET! CHECK: " + ToString());
        }

    }
}
