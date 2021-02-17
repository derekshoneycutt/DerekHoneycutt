using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GpxAnalysis
{
    public sealed class GpxAnalyzer
    {
        private GpsPoint GetPointFromXml(XElement xel)
        {
            var ret = new GpsPoint()
            {
                Elevation = 0.0d,
                Latitude = 0.0d,
                Longitude = 0.0d,
                Time = DateTime.Now
            };

            var lat = xel.Attributes().FirstOrDefault(xatt => String.Equals(xatt.Name.LocalName, "lat", StringComparison.InvariantCultureIgnoreCase));
            if (lat != null)
            {
                ret.Latitude = double.Parse(lat.Value);
            }
            var lon = xel.Attributes().FirstOrDefault(xatt => String.Equals(xatt.Name.LocalName, "lon", StringComparison.InvariantCultureIgnoreCase));
            if (lon != null)
            {
                ret.Longitude = double.Parse(lon.Value);
            }

            var ele = xel.Elements().FirstOrDefault(xele => String.Equals(xele.Name.LocalName, "ele", StringComparison.InvariantCultureIgnoreCase));
            if (ele != null)
            {
                ret.Elevation = double.Parse(ele.Value);
            }

            var time = xel.Elements().FirstOrDefault(xtime => String.Equals(xtime.Name.LocalName, "time", StringComparison.InvariantCultureIgnoreCase));
            if (time != null)
            {
                ret.Time = DateTime.Parse(time.Value);
            }

            return ret;
        }

        public IEnumerable<GpsPoint> MergeTrackPoints(IEnumerable<GpsTrack> tracks)
        {
            foreach (var track in tracks)
            {
                foreach (var point in track.Points)
                {
                    yield return point;
                }
            }
        }

        public IEnumerable<GpsTrack> GetTracks(string gpxFile)
        {
            var xdoc = XDocument.Load(gpxFile);
            
            if (String.Equals(xdoc.Root.Name.LocalName, "gpx", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var trk in 
                            from el in xdoc.Root.Elements()
                            where String.Equals(el.Name.LocalName, "trk", StringComparison.InvariantCultureIgnoreCase)
                            select el)
                {
                    var ret = new GpsTrack()
                    {
                        Name = String.Empty,
                        Points = new List<GpsPoint>()
                    };
                    
                    var name = trk.Elements().FirstOrDefault(xe => String.Equals(xe.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));
                    if (name != null)
                    {
                        ret.Name = name.Value;
                    }
                    
                    var points = from seg in trk.Elements()
                                 where String.Equals(seg.Name.LocalName, "trkseg", StringComparison.InvariantCultureIgnoreCase)
                                 from point in seg.Elements()
                                 where String.Equals(point.Name.LocalName, "trkpt", StringComparison.InvariantCultureIgnoreCase)
                                 let gps = GetPointFromXml(point)
                                 orderby gps.Time
                                 select gps;


                    ret.Points = points.ToList();

                    yield return ret;
                }
            }
        }


        private class VectorPoint
        {
            private const double RadiusEarth = 6371000d;

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

            public VectorPoint() { }

            public VectorPoint(GpsPoint point)
            {
                var el = point.Elevation + RadiusEarth;
                var latR = Radians(90 - point.Latitude);
                var lonR = Radians(point.Longitude);

                X = el * Math.Cos(lonR) * Math.Sin(latR);
                Y = el * Math.Sin(lonR) * Math.Sin(latR);
                Z = el * Math.Cos(latR);
            }

            private double Radians(double degrees)
            {
                return degrees * Math.PI / 180d;
            }

            public double DistanceFrom(VectorPoint vp)
            {
                return Math.Sqrt(
                    Math.Pow(X - vp.X, 2) +
                    Math.Pow(Y - vp.Y, 2) +
                    Math.Pow(Z - vp.Z, 2)) / 1000d;
            } 
        }

        private double DistanceBetween(GpsPoint first, GpsPoint second)
        {
            return (new VectorPoint(first)).DistanceFrom(new VectorPoint(second));
        }

        private double IfMetersToFeet(bool doIf, double m)
        {
            return doIf ? m * 3.28084d : m;
        }

        private double IfKilometersToMiles(bool doIf, double km)
        {
            return doIf ? km * 0.621371d : km;
        }

        public PlotGraph GetGraphFromTrack(GpsTrack trk, bool useImperial)
        {
            var ret = new PlotGraph()
            {
                Name = trk.Name,
                MaxElevation = 0.0d,
                MinElevation = 0.0d,
                TotalDistance = 0.0d,
                TotalElevationGain = 0.0d,
                TotalElevationLoss = 0.0d,
                Points = new List<PlotPoint>(trk.Points.Count)
            };

            if (trk.Points.Count < 1)
                return ret;

            var last = trk.Points[0];
            var first = new PlotPoint()
            {
                Distance = 0.0d,
                Elevation = IfMetersToFeet(useImperial, last.Elevation)
            };
            ret.MaxElevation = IfMetersToFeet(useImperial, last.Elevation);
            ret.MinElevation = IfMetersToFeet(useImperial, last.Elevation);

            ret.Points.Add(first);
            
            foreach (var point in trk.Points.Skip(1))
            {
                ret.TotalDistance += IfKilometersToMiles(useImperial, DistanceBetween(last, point));
                
                var eleChange = point.Elevation - last.Elevation;
                if (eleChange > 0)
                {
                    ret.TotalElevationGain += IfMetersToFeet(useImperial, eleChange);
                }
                else
                {
                    ret.TotalElevationLoss += IfMetersToFeet(useImperial, eleChange * -1);
                }

                var useElev = IfMetersToFeet(useImperial, point.Elevation);
                if (useElev > ret.MaxElevation)
                {
                    ret.MaxElevation = useElev;
                }
                if (useElev < ret.MinElevation)
                {
                    ret.MinElevation = useElev;
                }

                var newPoint = new PlotPoint()
                {
                    Distance = ret.TotalDistance,
                    Elevation = useElev
                };
                ret.Points.Add(newPoint);

                last = point;
            }

            return ret;
        }
    }
}
