using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class GpsPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GpsPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public string ToString()
        {
            return "Latitude: " + Latitude + "; Longitude: " + Longitude;
        }
    }
}
