using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalysis
{
    public sealed class PlotGraph
    {
        public string Name { get; set; }
        public double MaxElevation { get; set; }
        public double MinElevation { get; set; }
        public double TotalElevationGain { get; set; }
        public double TotalElevationLoss { get; set; }
        public double TotalDistance { get; set; }

        public List<PlotPoint> Points { get; set; }
    }
}
