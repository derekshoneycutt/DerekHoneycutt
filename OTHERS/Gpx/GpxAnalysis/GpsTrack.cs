using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalysis
{
    public sealed class GpsTrack
    {
        public string Name { get; set; }
        public List<GpsPoint> Points { get; set; }
    }
}
