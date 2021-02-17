using GpxAnalysis;
using GpxAnalyzer.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace GpxAnalyzer
{
    public sealed class TrackNameVM : NotifyingModel
    {
        public static PropertyChangedEventArgs TitleChangedArgs = new PropertyChangedEventArgs("Title");
        public static PropertyChangedEventArgs IsSelectedChangedArgs = new PropertyChangedEventArgs("IsSelected");

        private string m_Title;
        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                SetValue(ref m_Title, value, TitleChangedArgs);
            }
        }
        private bool m_IsSelected;
        public bool IsSelected
        {
            get
            {
                return m_IsSelected;
            }
            set
            {
                SetValue(ref m_IsSelected, value, IsSelectedChangedArgs);
            }
        }

        public TrackNameVM(string name = "", bool isSelected = false)
        {
            m_Title = name;
            m_IsSelected = isSelected;
        }

        public override string ToString()
        {
            return m_Title;
        }
    }

    public sealed class GpxAnalyzerVM : NotifyingModel
    {
        public static PropertyChangedEventArgs ElevLegendChangedArgs = new PropertyChangedEventArgs("ElevLegend");
        public static PropertyChangedEventArgs ElevIntervalChangedArgs = new PropertyChangedEventArgs("ElevInterval");
        public static PropertyChangedEventArgs DistIntervalChangedArgs = new PropertyChangedEventArgs("DistInterval");


        public static PropertyChangedEventArgs DistMaxChangedArgs = new PropertyChangedEventArgs("DistMax");
        public static PropertyChangedEventArgs ElevMinChangedArgs = new PropertyChangedEventArgs("ElevMin");
        public static PropertyChangedEventArgs ElevMaxChangedArgs = new PropertyChangedEventArgs("ElevMax");


        public static PropertyChangedEventArgs PointsChangedArgs = new PropertyChangedEventArgs("Points");
        public static PropertyChangedEventArgs TracksChangedArgs = new PropertyChangedEventArgs("Tracks");

        private IIODialog _openFileIO;
        private IIODialog _saveFileIO;

        private List<GpsTrack> CurrentTracks;
        private PlotGraph Graph;

        private string m_ElevLegend;
        public string ElevLegend
        {
            get
            {
                return m_ElevLegend;
            }
            set
            {
                SetValue(ref m_ElevLegend, value, ElevLegendChangedArgs);
            }
        }

        private int m_ElevInterval;
        public int ElevInterval
        {
            get
            {
                return m_ElevInterval;
            }
            set
            {
                if (SetValue(ref m_ElevInterval, value, ElevIntervalChangedArgs))
                {
                    SetMinMax();
                }
            }
        }

        private double m_DistInterval;
        public double DistInterval
        {
            get
            {
                return m_DistInterval;
            }
            set
            {
                if (SetValue(ref m_DistInterval, value, DistIntervalChangedArgs))
                {
                    SetMinMax();
                }
            }
        }

        private double m_DistMax;
        public double DistMax
        {
            get
            {
                return m_DistMax;
            }
            set
            {
                SetValue(ref m_DistMax, value, DistMaxChangedArgs);
            }
        }

        private int m_ElevMin;
        public int ElevMin
        {
            get
            {
                return m_ElevMin;
            }
            set
            {
                SetValue(ref m_ElevMin, value, ElevMinChangedArgs);
            }
        }

        private int m_ElevMax;
        public int ElevMax
        {
            get
            {
                return m_ElevMax;
            }
            set
            {
                SetValue(ref m_ElevMax, value, ElevMaxChangedArgs);
            }
        }

        private ObservableCollection<DataPoint> m_Points;
        public ObservableCollection<DataPoint> Points
        {
            get
            {
                return m_Points;
            }
            private set
            {
                SetValue(ref m_Points, value, PointsChangedArgs);
            }
        }

        private ObservableCollection<TrackNameVM> m_Tracks;
        public ObservableCollection<TrackNameVM> Tracks
        {
            get
            {
                return m_Tracks;
            }
            private set
            {
                if (SetValue(ref m_Tracks, value, TracksChangedArgs))
                {
                    RefreshGraph();
                }
            }
        }

        public ICommand OpenFileCmd { get; private set; }

        public ICommand SaveImageCmd { get; private set; }

        public GpxAnalyzerVM(IIODialog openFile, IIODialog saveFile)
        {
            _openFileIO = openFile;
            _saveFileIO = saveFile;
            Graph = null;
            CurrentTracks = new List<GpsTrack>();

            m_ElevLegend = String.Empty;
            m_ElevInterval = 100;
            m_DistInterval = 10d;
            m_ElevMin = 0;
            m_ElevMax = 0;
            m_DistMax = 0d;

            m_Points = new ObservableCollection<DataPoint>();
            m_Tracks = new ObservableCollection<TrackNameVM>();

            m_Tracks.CollectionChanged += M_Tracks_CollectionChanged;

            OpenFileCmd = new RelayCommand(OpenFile);
            SaveImageCmd = new RelayCommand<IImageHolder>(SaveImage);
        }

        private void M_Tracks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshGraph();
        }

        private void TrackName_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshGraph();
        }

        private void SetMinMax()
        {
            if (Graph != null)
            {
                ElevMin = ((int)Graph.MinElevation / 100) * 100;
                ElevMax = (((((int)Graph.MaxElevation - ElevMin) + ElevInterval) / ElevInterval) * ElevInterval) + ElevMin;
                DistMax = Graph.TotalDistance;
            }
        }

        public void RefreshGraph()
        {
            if (CurrentTracks.Count > 0)
            {
                var analyzer = new GpxAnalysis.GpxAnalyzer();

                var useTrack = analyzer.MergeTrackPoints(from track in CurrentTracks
                                                         join trackname in Tracks on track.Name equals trackname.Title
                                                         where trackname.IsSelected
                                                         select track);

                Graph = analyzer.GetGraphFromTrack(new GpsTrack() { Name = "all", Points = useTrack.ToList() }, true);

                Points = new ObservableCollection<DataPoint>(Graph.Points.Select(pp => new DataPoint() { X = pp.Distance, Y = pp.Elevation }));

                ElevLegend = String.Format(
@"Distance: {0:0.00}mi
.  
Min Elevation: {4:0.00}ft
Max Elevation: {5:0.00}ft
.  
Elevation Gain: {1:0.00}ft
Elevation Loss: {2:0.00}ft
Cumulative Change: {3:0.00}ft",
                    Graph.TotalDistance, Graph.TotalElevationGain, Graph.TotalElevationLoss,
                    (Graph.TotalElevationGain + Graph.TotalElevationLoss),
                    Graph.MinElevation, Graph.MaxElevation);

                SetMinMax();
            }
        }

        private TrackNameVM GenerateNewTrackName(string name, bool selected)
        {
            var ret = new TrackNameVM(name, selected);
            ret.PropertyChanged += TrackName_PropertyChanged;
            return ret;
        }

        public void OpenFile()
        {
            var gpxFile = _openFileIO.Show(new IODialogInfo()
                {
                    Title = "Open GPX File",
                    Filters = new[] { "GPX Files|*.gpx", "All Files|*.*" }
                });

            if (!String.IsNullOrEmpty(gpxFile))
            {
                var analyzer = new GpxAnalysis.GpxAnalyzer();

                CurrentTracks.Clear();
                CurrentTracks.AddRange(analyzer.GetTracks(gpxFile));

                foreach (var track in Tracks)
                {
                    track.PropertyChanged -= TrackName_PropertyChanged;
                }
                Tracks.CollectionChanged -= M_Tracks_CollectionChanged;

                Tracks = new ObservableCollection<TrackNameVM>(from track in CurrentTracks
                                                               select GenerateNewTrackName(track.Name, true));
                Tracks.CollectionChanged += M_Tracks_CollectionChanged;
            }
        }

        public void SaveImage(IImageHolder holder)
        {
            if (holder != null)
            {
                var file = _saveFileIO.Show(new IODialogInfo()
                {
                    Title = "Save Elevation Chart Image",
                    Filters = new[] { "PNG Images|*.png", "BMP Images|*.bmp" }
                });

                if (!String.IsNullOrEmpty(file))
                {
                    var imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                    if (file.EndsWith(".bmp"))
                    {
                        imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    }

                    var image = holder.GetImage(imageFormat);
                    image.Save(file, imageFormat);
                }
            }
        }
    }
}
