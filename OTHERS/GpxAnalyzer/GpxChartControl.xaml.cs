using GpxAnalyzer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.Collections.Specialized;
using GpxAnalyzer.DataTypes;

namespace GpxAnalyzer
{
    /// <summary>
    /// Interaction logic for GpxChartControl.xaml
    /// </summary>
    public partial class GpxChartControl : UserControl, IImageHolder
    {
        public GpxChartControl()
        {
            InitializeComponent();

            this.HandleDisposable(new CompositeDisposable(WinFormsChart, WinFormHost));
        }

        public static readonly DependencyProperty BackColorProperty =
            DependencyProperty.RegisterAttached("BackColor", typeof(System.Drawing.Color), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(System.Drawing.Color.White,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(BackColorChanged)));

        private static void BackColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            var value = e.NewValue as System.Drawing.Color?;
            if ((me != null) && value.HasValue)
            {
                me.WinFormsChart.BackColor = value.Value;
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                return (System.Drawing.Color)GetValue(BackColorProperty);
            }
            set
            {
                SetValue(BackColorProperty, value);
            }
        }

        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.RegisterAttached("PointsSource", typeof(ICollection<DataPoint>), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(PointsSourceChanged)));

        private static void PointsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;

            if (me != null)
            {
                me.FillPoints(e.NewValue as ICollection<DataPoint>);

                var obs = e.NewValue as INotifyCollectionChanged;
                if (obs != null)
                {
                    obs.CollectionChanged += me.Obs_CollectionChanged;
                }
                obs = e.OldValue as INotifyCollectionChanged;
                if (obs != null)
                {
                    obs.CollectionChanged -= me.Obs_CollectionChanged;
                }
            }
        }

        private void FillPoints(ICollection<DataPoint> src)
        {
            var series = WinFormsChart.Series["GpxElevation"];
            series.Points.Clear();
            if (src != null)
            {
                foreach (var point in src)
                {
                    series.Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint(point.X, point.Y));
                }
            }
        }

        private void Obs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FillPoints(PointsSource);
        }

        public ICollection<DataPoint> PointsSource
        {
            get
            {
                return (ICollection<DataPoint>)GetValue(PointsSourceProperty);
            }
            set
            {
                SetValue(PointsSourceProperty, value);
            }
        }

        public static readonly DependencyProperty LegendTextProperty =
            DependencyProperty.RegisterAttached("LegendText", typeof(string), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(String.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(LegendTextChanged)));

        private static void LegendTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;

            if (me != null)
            {
                var series = me.WinFormsChart.Series["GpxElevation"];
                series.LegendText = (string)e.NewValue;
            }
        }

        public string LegendText
        {
            get
            {
                return (string)GetValue(LegendTextProperty);
            }
            set
            {
                SetValue(LegendTextProperty, value);
            }
        }

        private void UpdateXAxis()
        {
            var chartArea = WinFormsChart.ChartAreas["Default"];
            chartArea.AxisX = new Axis() { Interval = XInterval, Minimum = XMinimum, Maximum = XMaximum, Title = XLabel };
        }

        public static readonly DependencyProperty XIntervalProperty =
            DependencyProperty.RegisterAttached("XInterval", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(XIntervalChanged)));

        private static void XIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateXAxis();
            }
        }

        public double XInterval
        {
            get
            {
                return (double)GetValue(XIntervalProperty);
            }
            set
            {
                SetValue(XIntervalProperty, value);
            }
        }

        public static readonly DependencyProperty XMinimumProperty =
            DependencyProperty.RegisterAttached("XMinimum", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(XMinimumChanged)));

        private static void XMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateXAxis();
            }
        }

        public double XMinimum
        {
            get
            {
                return (double)GetValue(XMinimumProperty);
            }
            set
            {
                SetValue(XMinimumProperty, value);
            }
        }



        public static readonly DependencyProperty XMaximumProperty =
            DependencyProperty.RegisterAttached("XMaximum", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(XMaximumChanged)));

        private static void XMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateXAxis();
            }
        }

        public double XMaximum
        {
            get
            {
                return (double)GetValue(XMaximumProperty);
            }
            set
            {
                SetValue(XMaximumProperty, value);
            }
        }



        public static readonly DependencyProperty XLabelProperty =
            DependencyProperty.RegisterAttached("XLabel", typeof(string), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(String.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(XLabelChanged)));

        private static void XLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateXAxis();
            }
        }

        public string XLabel
        {
            get
            {
                return (string)GetValue(XLabelProperty);
            }
            set
            {
                SetValue(XLabelProperty, value);
            }
        }

        private void UpdateYAxis()
        {
            var chartArea = WinFormsChart.ChartAreas["Default"];
            chartArea.AxisY = new Axis() { Interval = YInterval, Minimum = YMinimum, Maximum = YMaximum, Title = YLabel };
        }

        public static readonly DependencyProperty YIntervalProperty =
            DependencyProperty.RegisterAttached("YInterval", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(YIntervalChanged)));

        private static void YIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateYAxis();
            }
        }

        public double YInterval
        {
            get
            {
                return (double)GetValue(YIntervalProperty);
            }
            set
            {
                SetValue(YIntervalProperty, value);
            }
        }

        public static readonly DependencyProperty YMinimumProperty =
            DependencyProperty.RegisterAttached("YMinimum", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(YMinimumChanged)));

        private static void YMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateYAxis();
            }
        }

        public double YMinimum
        {
            get
            {
                return (double)GetValue(YMinimumProperty);
            }
            set
            {
                SetValue(YMinimumProperty, value);
            }
        }



        public static readonly DependencyProperty YMaximumProperty =
            DependencyProperty.RegisterAttached("YMaximum", typeof(double), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(YMaximumChanged)));

        private static void YMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateYAxis();
            }
        }

        public double YMaximum
        {
            get
            {
                return (double)GetValue(YMaximumProperty);
            }
            set
            {
                SetValue(YMaximumProperty, value);
            }
        }



        public static readonly DependencyProperty YLabelProperty =
            DependencyProperty.RegisterAttached("YLabel", typeof(string), typeof(GpxChartControl),
                new FrameworkPropertyMetadata(String.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(YLabelChanged)));

        private static void YLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as GpxChartControl;
            if (me != null)
            {
                me.UpdateYAxis();
            }
        }

        public string YLabel
        {
            get
            {
                return (string)GetValue(YLabelProperty);
            }
            set
            {
                SetValue(YLabelProperty, value);
            }
        }

        public System.Drawing.Image GetImage(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Image ret = null;
            using (var s = new System.IO.MemoryStream())
            {
                WinFormsChart.SaveImage(s, format);
                ret = System.Drawing.Image.FromStream(s);
            }

            return ret;
        }
    }
}
