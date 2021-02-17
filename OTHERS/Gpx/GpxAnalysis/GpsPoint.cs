using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalysis
{
    public sealed class GpsPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime Time { get; set; }
    }
}
