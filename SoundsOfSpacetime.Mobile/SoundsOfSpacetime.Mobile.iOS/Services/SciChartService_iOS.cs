using SciChart.iOS.Charting;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.iOS.Services;
using SoundsOfSpacetime.Mobile.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(SciChartService_iOS))]
namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class SciChartService_iOS : IPlotService
    {
        #region Fields

        /// <summary>
        /// The plotting surface
        /// </summary>
        private SCIChartSurface _plottingSurface;

        /// <summary>
        /// The data driving series drawn on the plotting surface
        /// </summary>
        private XyDataSeries<double, double> _series;

        /// <summary>
        ///The view drawing the data series   
        /// </summary>
        private SCIFastLineRenderableSeries _renderableSeries;

        /// <summary>
        /// The x-axis
        /// </summary>
        SCINumericAxis _xAxis;

        /// <summary>
        /// The y-axis
        /// </summary>
        SCINumericAxis _yAxis;

        /// <summary>
        /// The animation timer
        /// </summary>
        private PlotAnimationTimer _plotAnimationTimer;

        /// <summary>
        /// The animation delay time when performing animations
        /// </summary>
        private readonly double _animationDelayTime;

        /// <summary>
        /// AnimationLock
        /// </summary>
        private readonly object _plotAnimationLock;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Sets the necessary lifetime data
        /// </summary>
        public SciChartService_iOS()
        {
            //We're going to append some number of points and then wait the delay time to give the animation effect
            this._animationDelayTime = 50;

            //This timer will handle all of the waiting between point appendages to give the appearance of animation.
            this._plotAnimationTimer = new PlotAnimationTimer { Interval = this._animationDelayTime };
            this._plotAnimationTimer.Elapsed += OnPlotTimerElapsed;
            this._plotAnimationLock = new object();
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            this._series.Clear();
        }

        public void Plot(IEnumerable<IPoint> dataSeries)
        {
            this.StopIfAnimating();
            this.Clear();

            this._series.Append(dataSeries.Select(p => p.X), dataSeries.Select(p => p.Y));
            this._plottingSurface.ZoomExtents();
        }

        public void PlotAnimated(IEnumerable<IPoint> dataSeries, double desiredTimeInMillis)
        {
            this.StopIfAnimating();
            this.Clear();

            var dataSeriesEnumerated = dataSeries.ToList();
            int dataBatchSize = (int)(dataSeries.Count() / (desiredTimeInMillis / this._animationDelayTime));

            //Create a cache for the animated plot as we'll be plotting a few points and then waiting for some time to give the animated effect.
            PlotAnimationCache plotAnimationCache = new PlotAnimationCache(dataSeriesEnumerated, dataBatchSize);
            this._plotAnimationTimer.Cache = plotAnimationCache;
            this._plotAnimationTimer.Start();

            this._plottingSurface.ZoomExtents();
        }

        public View Render()
        {
            ////Create the surface
            //this._plottingSurface = new SCIChartSurface();
            //this._plottingSurface.BackgroundColor = UIColor.Black;

            ////Create the series
            //this._series = new XyDataSeries<double, double>();
            //this._series.AcceptUnsortedData = true;

            ////Creating the axes
            //this._xAxis = new SCINumericAxis()
            //{
            //    Style = new SCIAxisStyle()
            //    {
            //        DrawMinorGridLines = false,
            //        DrawMajorGridLines = false,
            //        DrawMajorBands = false,
            //    },
            //    GrowBy = new SCIDoubleRange(0.1d, 0.1d)
            //};

            //this._yAxis = new SCINumericAxis()
            //{
            //    Style = new SCIAxisStyle()
            //    {
            //        DrawMinorGridLines = false,
            //        DrawMajorGridLines = false,
            //        DrawMajorBands = false,
            //    },
            //    GrowBy = new SCIDoubleRange(0.1d, 0.1d)
            //};

            ////The Renderable Series
            //this._renderableSeries = new SCIFastLineRenderableSeries { DataSeries = this._series, StrokeStyle = new SCISolidPenStyle(0xFF279B27, 2f) };
            //var pointMarker = new SCIEllipsePointMarker { StrokeStyle = new SCISolidPenStyle(UIColor.Red, 0.5f), FillStyle = new SCISolidBrushStyle(0xFFFFA300) };
            //this._renderableSeries.StrokeStyle = new SCISolidPenStyle(new UIColor(new nfloat(255/255), new nfloat(64/255), new nfloat(129/255), new nfloat(255/255)), 2f);

            ////Adding this stuff to the surface
            //using (this._plottingSurface.SuspendUpdates())
            //{
            //    this._plottingSurface.XAxes.Add(this._xAxis);
            //    this._plottingSurface.YAxes.Add(this._yAxis);
            //    this._plottingSurface.RenderableSeries.Add(this._renderableSeries);
            //    this._plottingSurface.ChartModifiers = new SCIChartModifierCollection
            //    {
            //        new SCIZoomPanModifier(),
            //        new SCIPinchZoomModifier(),
            //        new SCIZoomExtentsModifier(),
            //    };
            //}

            ////Returns the native plot as a Forms View
            //return this._plottingSurface.ToView();
            this._plottingSurface = new SCIChartSurface();
            this._plottingSurface.TranslatesAutoresizingMaskIntoConstraints = true;
            this._series = new XyDataSeries<double, double>();
            var dataseries2 = new SCIXyyDataSeries();
            this._series.AcceptUnsortedData = true;

            var xAxisTitleStyle = new SCITextFormattingStyle { AlignmentHorizontal = SCILabelAlignmentHorizontalMode.Center };
            var xAxisStyle = new SCIAxisStyle { DrawMajorGridLines = false, DrawMinorGridLines = false, DrawMajorBands = false, AxisTitleLabelStyle = xAxisTitleStyle };
            this._xAxis = new SCINumericAxis { AxisTitle = "Time", Style = xAxisStyle };

            var yAxisTitleStyle = new SCITextFormattingStyle { AlignmentVertical = SCILabelAlignmentVerticalMode.Center };
            var yAxisStyle = new SCIAxisStyle { DrawMajorGridLines = false, DrawMinorGridLines = false, DrawMajorBands = false, AxisTitleLabelStyle = yAxisTitleStyle };
            this._yAxis = new SCINumericAxis { AxisTitle = "H(T)", Style = yAxisStyle };

            this._renderableSeries = new SCIFastLineRenderableSeries { DataSeries = this._series, StrokeStyle = new SCISolidPenStyle(0xFF279B27, 2f) };
            this._renderableSeries.StrokeStyle = new SCISolidPenStyle(UIColor.FromRGB(255, 64, 129), 2f);
            using (this._plottingSurface.SuspendUpdates())
            {
                this._plottingSurface.XAxes.Add(this._xAxis);
                this._plottingSurface.YAxes.Add(this._yAxis);
                this._plottingSurface.RenderableSeries.Add(this._renderableSeries);
                this._plottingSurface.ChartModifiers = new SCIChartModifierCollection
                {
                    new SCIZoomPanModifier(),
                    new SCIPinchZoomModifier(),
                    new SCIZoomExtentsModifier(),
                };
            }

            return this._plottingSurface.ToView();
            //bottomLayout.Children.Add(Surface.ToView(),
            //widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
            //heightConstraint: Constraint.RelativeToParent(parent => parent.Height));
        }

        public void SetTitle(string title)
        {
        }

        public void SetXAxisTitle(string xAxistTitle)
        {
            this._xAxis.AxisTitle = xAxistTitle;
        }   


        public void SetYAxisTitle(string yAxisTitle)
        {
            this._yAxis.AxisTitle = yAxisTitle;
        }

        #endregion

        #region Non-Public Methods

        /// <summary>
        /// Fascilitates calls necessary to draw the next batch of points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlotTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.DrawNextBatchOfPoints();
        }

        /// <summary>
        /// Draws the next batch of points
        /// </summary>
        private void DrawNextBatchOfPoints()
        {
            lock (this._plotAnimationLock)
            {
                //If the data necessary for drawing the next batch of points isn't valid... return
                if (this._plotAnimationTimer.Cache == null || this._plotAnimationTimer.Cache?.DataSeries == null || this._plotAnimationTimer.Cache?.BatchSize <= 0)
                    return;

                //Get and append the next batch of points
                var nextBatchOfPoints = this._plotAnimationTimer.Cache.DataSeries.GetRange(this._plotAnimationTimer.Cache.Offset, this._plotAnimationTimer.Cache.BatchSize).ToList();
                this._series.Append(nextBatchOfPoints.Select(point => point.X), nextBatchOfPoints.Select(point => point.Y));
                this._plottingSurface.ZoomExtents();
                this._plotAnimationTimer.Cache.Offset = this._plotAnimationTimer.Cache.Offset + this._plotAnimationTimer.Cache.BatchSize;

                if (this._plotAnimationTimer.Cache.Offset >= this._plotAnimationTimer.Cache.DataSeries.Count)
                {
                    this._plotAnimationTimer.Stop();
                    return;
                }
            }

        }

        /// <summary>
        /// Stops the current animation timer
        /// </summary>
        private void StopIfAnimating()
        {
            this._plotAnimationTimer.Stop();
        }
        #endregion
    }
}