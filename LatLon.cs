using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic_Flood
{
    class LatLon
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LatLon(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
