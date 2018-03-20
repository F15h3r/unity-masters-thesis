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

        private string tag { get; set; }
        private RESTServicesManager rsm;

        public Marker()
        {
            longitude = latitude = altitude = float.MinValue;
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
            if (altitude != float.MinValue || longitude != float.MinValue)
            {
                if (rsm == null)
                    rsm = new RESTServicesManager();
                altitude = rsm.getElevation(longitude, latitude);
            }
            else
                Debug.Log("LONGITUDE OR ALTITUDE NOT SET! CHECK: " + ToString());
        }

        public void translateRelativeToUser(float uLon, float uLat)
        {
            Debug.Log("TODO");
        }
    }
}
